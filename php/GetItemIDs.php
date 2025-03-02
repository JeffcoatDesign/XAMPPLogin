<?php
require 'ConnectionSettings.php';

//User submitted variables
$userID = $_POST["userID"];

//Check Connection
if($conn->connect_error) {
	die("Connection failed: " . $conn->connect_error);
}
$sql = "SELECT ID, itemID from usersitems WHERE userID = '". $userID . "'";
$result = $conn->query($sql);

if($result->num_rows > 0) {
	//	output data of each row
	$rows = array();
	while($row = $result->fetch_assoc()) {
		$rows[] = $row;
	}
	// After the array is created
	echo json_encode($rows);
} else {
	echo "0";
}
$conn->close();
?>