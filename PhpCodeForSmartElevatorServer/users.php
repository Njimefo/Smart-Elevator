<?php
/**
 * Created by PhpStorm.
 * User: Brandon Njimefo
 * Date: 26.05.2018
 * Time: 23:33
 */
require 'classes/UserManager.php';
require 'classes/response.php';
require 'classes/LoggingResult.php';

$answer = new response();
if (strcasecmp($_SERVER['REQUEST_METHOD'], 'POST') != 0) {
    $answer->Executed = false;
    $answer->Result = "The Request must method must be post";
} else {
    $contentType = isset($_SERVER["CONTENT_TYPE"]) ? trim($_SERVER["CONTENT_TYPE"]) : '';
    if (strcasecmp($contentType, 'application/json') != 0) {

        $answer->Executed = false;
        $answer->Result = "The content type must be like this : application/json";
    } else {
        //Receive the RAW post data.
        $content = trim(file_get_contents("php://input"));

//Attempt to decode the incoming RAW post data from JSON.
        $loginUser = json_decode($content, true);
        if (!is_array($loginUser)) {
            $answer->Executed = false;
            $answer->Result = "The Data you sent have bad format";

        } else if (!isset($loginUser["Username"]) || empty($loginUser["Username"])  ) {
      
$answer->Executed = false;
            $answer->Result = "One or more parameters are in bad format";

        } else {
            $userData = new UserManager();
            $answer->Executed = true;
            $user = new User();
                       $user->Username= $loginUser["Username"];
            $user = $userData->GetUser($user->Username, false);
date_default_timezone_set("UTC");
        $dt = new DateTime();

            if ($user->CurrentToken != null && $user->CurrentToken->Value != null && $user->CurrentToken->Value == $loginUser["Token"]) {
                $interval = date_diff($dt, $user->CurrentToken->ExpireDate);
                if ($interval->m < 1 && $interval->s < 1) {
                    $answer->Executed = false;
                    $answer->Result = "User logged out";
                } else {
                    if ($loginUser["Action"] == 0) {
                        $userData->UpdateElevator($user->Username, $loginUser["Elevator"]);
                        $answer->Executed = true;
                        $answer->Result =$userData->GetUser($user->Username, false);;
                    } else if ($loginUser["Action"] == 1) {
                        $userData->UpdateUserGoto($loginUser["Username"], $loginUser["Goto"]);
                        $answer->Executed = true;
                        $answer->Result = "Location added";
                    } else {
                        $answer->Executed = false;
                        $answer->Result = "Action Id not correct";
                    }
                }
            }
            else
            {
                
                    $answer->Executed = false;
                    $answer->Result = "User not logged or bad token";
                
            }
            
        }


    }
}
$answer = json_encode($answer);
echo $answer;
?>
