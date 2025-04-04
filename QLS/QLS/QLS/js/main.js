document.addEventListener('DOMContentLoaded', function () {
    fetchProducts();
    document.getElementById('btnAdd').addEventListener('click', addProduct);
    document.getElementById('btnUpdate').addEventListener('click', updateProduct);
});

const apiUrl = 'http://localhost:5007/api/ProductApi';
let selectedProductId = null; // Biến lưu ID sản phẩm đang sửa

// Lấy danh sách sản phẩm
function fetchProducts() {
    fetch(apiUrl)
        .then(handleResponse)
        .then(data => displayProducts(data))
        .catch(error => console.error('Fetch error:', error.message));
}

// Xử lý phản hồi fetch
function handleResponse(response) {
    if (!response.ok) throw new Error('Network response was not ok');
    return response.json();
}

// Hiển thị danh sách sản phẩm
function displayProducts(products) {
    const productList = document.getElementById('productList');
    productList.innerHTML = ''; // Xóa danh sách cũ

    products.forEach(product => {
        productList.innerHTML += createProductRow(product);
    });

    // Thêm sự kiện cho các nút
    document.querySelectorAll('.delete-btn').forEach(button => {
        button.addEventListener('click', () => deleteProduct(button.dataset.id));
    });
    document.querySelectorAll('.edit-btn').forEach(button => {
        button.addEventListener('click', () => editProduct(button.dataset.id));
    });
    document.querySelectorAll('.view-btn').forEach(button => {
        button.addEventListener('click', () => viewProduct(button.dataset.id));
    });
}

// Tạo dòng sản phẩm
function createProductRow(product) {
    return `
        <tr>
            <td>${product.id}</td>
            <td>${product.name}</td>
            <td>${product.price}</td>
            <td>${product.description}</td>
            <td>
                <button class="btn btn-danger delete-btn" data-id="${product.id}">Xóa</button>
                <button class="btn btn-warning edit-btn" data-id="${product.id}">Sửa</button>
                <button class="btn btn-primary view-btn" data-id="${product.id}">Xem</button>
            </td>
        </tr>
    `;
}

// Thêm sản phẩm
function addProduct() {
    const productData = {
        name: document.getElementById('bookName').value,
        price: document.getElementById('price').value,
        description: document.getElementById('description').value
    };

    fetch(apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(productData)
    })
        .then(handleResponse)
        .then(() => {
            fetchProducts(); // Refresh danh sách
            resetForm();
        })
        .catch(error => console.error('Error:', error));
}

// Xóa sản phẩm
function deleteProduct(productId) {
    fetch(`${apiUrl}/${productId}`, {
        method: 'DELETE',
    })
        .then(response => {
            if (response.status === 204) {
                console.log('Sản phẩm đã bị xóa.');
                fetchProducts(); // Cập nhật lại danh sách
            } else {
                console.error('Xóa sản phẩm thất bại.');
            }
        })
        .catch(error => console.error('Error:', error));
}

// Xem sản phẩm chi tiết
function viewProduct(productId) {
    fetch(`${apiUrl}/${productId}`)
        .then(handleResponse)
        .then(product => {
            alert(`Tên: ${product.name}\nGiá: ${product.price}\nMô tả: ${product.description}`);
        })
        .catch(error => console.error('Error:', error));
}

// Sửa sản phẩm (đưa thông tin vào form)
function editProduct(productId) {
    fetch(`${apiUrl}/${productId}`)
        .then(handleResponse)
        .then(product => {
            // Điền thông tin vào form
            document.getElementById('bookName').value = product.name;
            document.getElementById('price').value = product.price;
            document.getElementById('description').value = product.description;

            // Lưu lại ID sản phẩm đang sửa
            selectedProductId = productId;

            // Ẩn nút "Thêm", Hiện nút "Cập nhật"
            document.getElementById('btnAdd').style.display = 'none';
            document.getElementById('btnUpdate').style.display = 'inline-block';
        })
        .catch(error => console.error('Error:', error));
}

// Cập nhật sản phẩm
function updateProduct() {
    if (!selectedProductId) return;

    const updatedProduct = {
        id: selectedProductId,
        name: document.getElementById('bookName').value,
        price: document.getElementById('price').value,
        description: document.getElementById('description').value
    };

    fetch(`${apiUrl}/${selectedProductId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedProduct)
    })
        .then(response => {
            if (response.status === 204) {
                console.log('Cập nhật sản phẩm thành công.');
                fetchProducts(); // Refresh danh sách
                resetForm();
            } else {
                console.error('Cập nhật sản phẩm thất bại.');
            }
        })
        .catch(error => console.error('Error:', error));
}

// Reset form
function resetForm() {
    document.getElementById('bookName').value = '';
    document.getElementById('price').value = '';
    document.getElementById('description').value = '';

    // Ẩn nút "Cập nhật", Hiện lại nút "Thêm"
    document.getElementById('btnAdd').style.display = 'inline-block';
    document.getElementById('btnUpdate').style.display = 'none';

    selectedProductId = null; // Reset ID sản phẩm đang sửa
}