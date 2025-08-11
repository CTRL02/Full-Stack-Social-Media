from flask import Flask, request, jsonify
from sqlalchemy import create_engine, text
import pandas as pd
import numpy as np
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
from flask_cors import CORS

app = Flask(__name__)
CORS(app, resources={r"/recommend": {"origins": "http://localhost:4200"}})

connection_string = (
    "mssql+pyodbc:///?"
    "odbc_connect=DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=(localdb)\\MyLocalDBInstance;"
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
        SELECT p.Id, p.Content, u.UserName, u.avatar , p.CreatedAt
        FROM Posts p
        JOIN Users u ON p.AppUserId = u.Id
    """)
    return pd.read_sql(query, conn)

def recommend_posts(username, top_n=5):
    with engine.connect() as conn:
        user_id = get_user_id(conn, username)
        if not user_id:
            return []

        liked_posts = get_user_liked_posts(conn, user_id)
        if liked_posts.empty:
            return []

        all_posts = get_all_posts(conn)

    tfidf = TfidfVectorizer(stop_words="english")
    tfidf_matrix = tfidf.fit_transform(all_posts['Content'])
    liked_tfidf = tfidf.transform(liked_posts['Content'])

    user_vector = np.asarray(liked_tfidf.mean(axis=0)).reshape(1, -1)

    similarities = cosine_similarity(user_vector, tfidf_matrix).flatten()
    all_posts['similarity'] = similarities

    recommendations = all_posts[all_posts['UserName'] != username]
    top_recs = recommendations.sort_values(by='similarity', ascending=False).head(top_n)

    return top_recs.to_dict(orient="records")

@app.route("/recommend", methods=["GET"])
def recommend():
    username = request.args.get("username")
    top_n = request.args.get("top_n", default=5, type=int)

    if not username:
        return jsonify({"error": "username parameter is required"}), 400

    results = recommend_posts(username, top_n=top_n)
    return jsonify(results)

if __name__ == "__main__":
    app.run(debug=True)
