<?php
    include_once 'db.php';
    $stage = $_POST["stage"];
    $queryleaderboard = "SELECT username, best_score, best_time FROM stage".$stage." ORDER BY best_score DESC, best_time ASC";
    $st = $con->prepare($queryleaderboard);
    $st->execute();
    $all = $st->fetchAll();
    $i = 1;
    foreach ($all as $row) {
        if($i == 5){
            echo $i . "\t" . $row["username"] . "\t" . $row["best_score"] . "\t" . $row["best_time"];
            break;
        }
        echo $i . "\t" . $row["username"] . "\t" . $row["best_score"] . "\t" . $row["best_time"] . ",";
        $i++;
    }
?>