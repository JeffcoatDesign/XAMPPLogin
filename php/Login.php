<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unitybackendtutorial";

//Variables submitted by username
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

$conn = new mysqli($servername, $username, $password, $dbname);

if($conn->connect_error) {
	die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT password, ID FROM users WHERE username = '" . $loginUser ."'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  while($row = $result->fetch_assoc()) {
    if ($row["password"] == $loginPass) {
		echo $row["ID"];
		//Get the user's data

		//Get player info.

		//Get inventory

		//Modify player data

		//update inventory
	}
	else {
		echo "Wrong Credentials";
	}
  }
} else {
  echo "Username does not exist.";
}

$conn->close();
?>