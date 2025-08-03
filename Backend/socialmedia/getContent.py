import os
from pathlib import Path
import re

# === Configuration ===
folder_names = ['Services', 'Resources', 'Repositories','Middleware','Entities','DTOs', 'Data','Controllers']  # Replace with your top-level folder names
output_dir = 'output_parts'            # Folder to save parts
max_words = 3000                       # Max words per split file
merged_filename = 'merged.txt'


def clean_text(text):
    return text.strip().replace('\r', '').replace('\n', ' ')


def read_all_files_recursively(folders):
    all_contents = []

    for folder in folders:
        folder_path = Path(folder)
        if not folder_path.exists():
            print(f"Folder not found: {folder}")
            continue

        for file in folder_path.rglob("*.*"):  # Recursive glob
            if file.is_file():
                try:
                    with open(file, 'r', encoding='utf-8') as f:
                        content = f.read().strip()
                        word_count = len(re.findall(r'\w+', content))
                        if word_count == 0:
                            continue
                        all_contents.append({
                            "filename": str(file.relative_to(folder_path)),
                            "content": content,
                            "word_count": word_count
                        })
                except Exception as e:
                    print(f"Failed to read {file}: {e}")

    return all_contents


def write_merged_file(all_contents):
    with open(merged_filename, 'w', encoding='utf-8') as out:
        for item in all_contents:
            out.write(f"\n--- {item['filename']} ---\n")
            out.write(f"{item['content']}\n")


def split_into_parts(all_contents):
    os.makedirs(output_dir, exist_ok=True)
    part_num = 1
    current_words = 0
    buffer = []

    for item in all_contents:
        # Start new part if the current file would exceed the word limit
        if current_words + item['word_count'] > max_words:
            write_part_file(buffer, part_num)
            part_num += 1
            buffer = []
            current_words = 0

        buffer.append(item)
        current_words += item['word_count']

    # Write the last part
    if buffer:
        write_part_file(buffer, part_num)


def write_part_file(contents, part_number):
    filename = os.path.join(output_dir, f'part_{part_number}.txt')
    with open(filename, 'w', encoding='utf-8') as out:
        for item in contents:
            out.write(f"\n--- {item['filename']} ---\n")
            out.write(f"{item['content']}\n")
    print(f"Written: {filename}")


def main():
    print("Reading files recursively...")
    all_contents = read_all_files_recursively(folder_names)

    print(f"Found {len(all_contents)} valid text files.")
    
    print("Writing merged file...")
    write_merged_file(all_contents)

    print("Splitting into parts...")
    split_into_parts(all_contents)

    print("Done!")


if __name__ == "__main__":
    main()
