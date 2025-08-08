from sqlalchemy import create_engine, text
import pandas as pd
import numpy as np
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity


connection_string = (
    "mssql+pyodbc:///?"
    "odbc_connect=DRIVER={ODBC Driver 17 for SQL Server};"
    r"SERVER=np:\\.\pipe\LOCALDB#D83266DC\tsql\query;"
    "DATABASE=socialmediapp;"
    "Trusted_Connection=yes;"
    "MARS_Connection=yes"
)

engine = create_engine(connection_string)

def get_user_id(conn, username):
    result = conn.execute(
        text("SELECT Id FROM Users WHERE UserName = :uname"),
        {"uname": username}
    ).fetchone()
    return result[0] if result else None


def get_user_liked_posts(conn, user_id):
    query = text("""
        SELECT p.Id, p.Content, u.UserName
        FROM Posts p
        JOIN Impressions i ON i.PostId = p.Id
        JOIN Users u ON p.AppUserId = u.Id
        WHERE i.AppUserId = :uid
          AND i.Type IN (0, 1, 2)
          AND i.CommentId IS NULL
    """)
    return pd.read_sql(query, conn, params={"uid": user_id})


def get_all_posts(conn):
    query = text("""
        SELECT p.Id, p.Content, u.UserName, u.avatar
        FROM Posts p
        JOIN Users u ON p.AppUserId = u.Id
    """)
    return pd.read_sql(query, conn)

def recommend_posts(username, top_n=5):
    with engine.connect() as conn:
        user_id = get_user_id(conn, username)
        if not user_id:
            print("User not found.")
            return pd.DataFrame()

        liked_posts = get_user_liked_posts(conn, user_id)
        if liked_posts.empty:
            print("No liked posts for this user.")
            return pd.DataFrame()

        all_posts = get_all_posts(conn)

    tfidf = TfidfVectorizer(stop_words="english")
    tfidf_matrix = tfidf.fit_transform(all_posts['Content'])
    liked_tfidf = tfidf.transform(liked_posts['Content'])

    user_vector = np.asarray(liked_tfidf.mean(axis=0)).reshape(1, -1)

    similarities = cosine_similarity(user_vector, tfidf_matrix).flatten()
    all_posts['similarity'] = similarities

    recommendations = all_posts[all_posts['UserName'] != username]
    return recommendations.sort_values(by='similarity', ascending=False).head(top_n)

recs = recommend_posts("user7", top_n=8)
pd.set_option("display.max_columns", None)
pd.set_option("display.width", None)      
pd.set_option("display.max_rows", None)     
print(recs)

