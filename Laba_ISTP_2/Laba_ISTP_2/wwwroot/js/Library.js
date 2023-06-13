const libraryUri = 'api/Libraries';
const bookUri = 'api/Books';
let libraries = [];

function getLibraries() {
    fetch(libraryUri)
        .then(response => response.json())
        .then(data => {
            libraries = data;
            _displayLibraries(data);
        })
        .catch(error => console.error('Unable to get libraries.', error));
}

function addLibrary() {
    const addNameTextbox = document.getElementById('add-name');
    const addLocationTextbox = document.getElementById('add-location');
    const library = {
        name: addNameTextbox.value.trim(),
        location: addLocationTextbox.value.trim(),
    };
    fetch(libraryUri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(library)
    })
        .then(response => response.json())
        .then(() => {
            getLibraries();
            addNameTextbox.value = '';
            addLocationTextbox.value = '';
        })
        .catch(error => console.error('Unable to add library.', error));
}

function deleteLibrary(id) {
    fetch(`${libraryUri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getLibraries())
        .catch(error => console.error('Unable to delete library.', error));
}

function displayEditForm(id) {
    const library = libraries.find(library => library.id === id);
    document.getElementById('edit-id').value = library.id;
    document.getElementById('edit-name').value = library.name;
    document.getElementById('edit-location').value = library.location;
    document.getElementById('editForm').style.display = 'block';
}

function updateLibrary() {
    const libraryId = document.getElementById('edit-id').value;
    const library = {
        id: parseInt(libraryId, 10),
        name: document.getElementById('edit-name').value.trim(),
        location: document.getElementById('edit-location').value.trim()
    };
    fetch(`${libraryUri}/${libraryId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(library)
    })
        .then(() => getLibraries())
        .catch(error => console.error('Unable to update library.', error));
    closeInput();
    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayLibraries(data) {
    const tableBody = document.getElementById('libraries');
    tableBody.innerHTML = '';

    data.forEach(library => {
        let booksHtml = '';
        if (library.books && library.books.length > 0) {
            library.books.forEach(book => {
                booksHtml += `<li>${book.title} by ${book.author}</li>`;
            });
        } else {
            booksHtml = '<li>No books available</li>';
        }

        const editButton = `<button onclick="displayEditForm(${library.id})">Edit</button>`;
        const deleteButton = `<button onclick="deleteLibrary(${library.id})">Delete</button>`;
        const row = `
            <tr>
                <td>${library.name}</td>
                <td>${library.location}</td>
                <td>
                    <ul>${booksHtml}</ul>
                    <form onsubmit="addBook(event, ${library.id})">
                        <input type="text" placeholder="Title" id="book-title-${library.id}" required>
                        <input type="text" placeholder="Author" id="book-author-${library.id}" required>
                        <input type="text" placeholder="Genre" id="book-genre-${library.id}" required>
                        <input type="text" placeholder="Year" id="book-year-${library.id}" required>
                        <input type="submit" value="Add Book">
                    </form>
                </td>
                <td>${editButton}</td>
                <td>${deleteButton}</td>
            </tr>
        `;
        tableBody.innerHTML += row;
    });
}


