<?php
    include_once 'db.php';
    if(isset($_POST["username"]) && !empty($_POST["username"]) && isset($_POST["password"]) && !empty($_POST["password"])) {
        Login($_POST['username'], $_POST['password']);
    }
    function Login($username, $password) {
        GLOBAL $con;

        $sql= "SELECT * FROM usersinfo WHERE username =? AND password = ?";
        $st=$con->prepare($sql);
        $st->execute(array($username, sha1($password)));
        $all=$st->fetchAll();
        if(count($all)==1){
            echo $all[0]['username'] ."\t" . $all[0]['password']."\t".$all[0]['lastedStage'];
            exit();
        }
        echo "SERVER: error, invalid username or password";
        exit();
    }

    echo "SERVER: error, enter a valid username and password";

?>