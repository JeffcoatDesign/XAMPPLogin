<?php
require 'ConnectionSettings.php';

//Variables submitted by user
//$loginUser = $_POST["loginUser"];
//$loginPass = $_POST["loginPass"];
$id = $_POST["id"];
$itemID = $_POST["itemID"];
$userID = $_POST["userID"];

if($conn->connect_error) {
	die("Connection failed: " . $conn->connect_error);
}

//First SQL
$sql = "SELECT price FROM items WHERE ID = '" . $itemID . "'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  $itemPrice = $result->fetch_assoc()["price"];

  $sql2 = "DELETE FROM usersitems WHERE ID = '" . $id . "'";
  $result2 = $conn->query($sql2);
  if($result2) {
	  //If deleted successfully
	  $sql3 = "UPDATE `users` SET `coins` = coins + '" . $itemPrice . "' WHERE `ID` = '" . $userID . "'";
	  $conn->query($sql3);
  }
  else {
	  echo "error: could not delete item";
  }
} else {
	echo "0";
}

$conn->close();
?>