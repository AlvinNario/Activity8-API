<?php
header("Content-Type: application/json");

$host = 'localhost';
$db = 'mydatabase';
$user = 'root';
$pass = '';

$dsn = "mysql:host=$host;dbname=$db;charset=utf8mb4";
$options = [
    PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
    PDO::ATTR_EMULATE_PREPARES => false,
];

try {
    $pdo = new PDO($dsn, $user, $pass, $options);
} catch (PDOException $e) {
    echo json_encode(['error' => $e->getMessage()]);
    exit;
}

if ($_SERVER['REQUEST_METHOD'] === 'GET') {
    $usersStmt = $pdo->query('SELECT id, username, email FROM users');
    $users = $usersStmt->fetchAll();

    $productsStmt = $pdo->query('SELECT id, name, price FROM products');
    $products = $productsStmt->fetchAll();

    echo json_encode(['users' => $users, 'products' => $products]);
} elseif ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $input = json_decode(file_get_contents('php://input'), true);

    // Insert into users table
    $userSql = 'INSERT INTO users (username, pass, email) VALUES (?, ?, ?)';
    $userStmt = $pdo->prepare($userSql);
    $userStmt->execute([$input['username'], $input['pass'], $input['email']]);
    $userId = $pdo->lastInsertId();

    // Insert into products table
    $productSql = 'INSERT INTO products (name, price) VALUES (?, ?)';
    $productStmt = $pdo->prepare($productSql);
    $productStmt->execute([$input['product_name'], $input['product_price']]);

    echo json_encode(['message' => 'User and product added successfully']);
}
?>