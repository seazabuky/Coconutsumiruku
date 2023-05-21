<?php
    include_once "db.php";
$stage = $_POST["stage"];
$username = $_POST["username"];
$password = sha1(sha1($_POST["password"]));
$score = $_POST["score"];
$time = $_POST["time"];

// $stage = 3;
// $username = "admin";
// $password = sha1(sha1(sha1("1234")));
// $score = 100;
// $time = "00:00:00";


    $keycheckquery = "SELECT keycheck FROM checkplayer WHERE username = :username";
    $st = $con->prepare($keycheckquery);
    $st->bindParam(":username", $username);
    $st->execute();
    $all = $st->fetchAll();
    if (count($all) != 0) {
        $check_update = "SELECT best_score,best_time FROM stage" . $stage . " WHERE username = :username";
        $st = $con->prepare($check_update);
        $st->bindParam(":username", $username);
        $st->execute();
        $all = $st->fetchAll();
        if (count($all) != 0) {
            $old_score = $all[0]["best_score"];
            $old_time = $all[0]["best_time"];
            if (($old_score < $score && strtotime($old_time) > strtotime($time))||($old_score < $score && strtotime($old_time) <= strtotime($time))||($old_score == $score && strtotime($old_time) > strtotime($time))) {
                $update_table = "UPDATE stage".$stage." SET best_score = :score, best_time = :time WHERE username = :username";
                $st = $con->prepare($update_table);
                $st->bindParam(':username', $username);
                $st->bindParam(':score', $score);
                $st->bindParam(':time', $time);
                $st->execute();
                echo "success";
            }else{
                echo "score and time not better";
            }
        }else{
            $insert_table = "INSERT INTO stage".$stage." (username, best_score, best_time) VALUES (:username, :score, :time)";
            $st = $con->prepare($insert_table);
            $st->bindParam(':username', $username);
            $st->bindParam(':score', $score);
            $st->bindParam(':time', $time);
            $st->execute();
            checkStage();
            echo "success";
        }
    }

    function checkStage(){
        GLOBAL $con, $username, $stage;
        $checkStage = "SELECT lastedStage FROM usersinfo WHERE username = :username";
        $st = $con->prepare($checkStage);
        $st->bindParam(":username", $username);
        $st->execute();
        $all = $st->fetchAll();
        if (count($all) != 0) {
            $lastedStage = $all[0]["lastedStage"];
            if ((int)$lastedStage < (int)$stage+1) {
                $stage = (int)$stage+1;
                $updateStage = "UPDATE usersinfo SET lastedStage = :stage WHERE username = :username";
                $st = $con->prepare($updateStage);
                $st->bindParam(":username", $username);
                $st->bindParam(":stage", $stage);
                $st->execute();
                echo "lastedStage updated \t";
            }
        }
    }
?>