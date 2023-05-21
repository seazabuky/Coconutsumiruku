<?php
    include_once 'db.php';
    $username = $_POST["username"];
    $password = sha1($_POST["password"]);
    $keycheck= sha1(sha1($username . $password));
    $namecheckquery = "SELECT username FROM usersinfo WHERE username = ?";
    $st=$con->prepare($namecheckquery);
    $st->execute(array($username));
    $all = $st->fetchAll();
    if (count($all) == 0) {
        $insertuserquery = "INSERT INTO usersinfo (username, password) VALUES (:username, :password)";
        $st = $con->prepare($insertuserquery);
        $st->bindParam(':username', $username);
        $st->bindParam(':password', $password);
        $st->execute();
        $insertcheckkey = "INSERT INTO checkplayer (username,keycheck) VALUES (:username,:keycheck)";
        $st = $con->prepare($insertcheckkey);
        $st->bindParam(':username', $username);
        $st->bindParam(':keycheck', $keycheck);
        $st->execute();
        echo $username . "\t" . $password . " \t 1";
        
    }
    else{
        echo "Server: error , name already exists";
    }

?>