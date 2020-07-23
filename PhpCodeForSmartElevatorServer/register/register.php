<?php
/**
 * Created by PhpStorm.
 * User: Brandon Njimefo
 * Date: 27.05.2018
 * Time: 00:10
 */

require '../classes/UserManager.php';
require '../classes/response.php';
require '../classes/RegisterResult.php';

$answer = new response();
if(strcasecmp($_SERVER['REQUEST_METHOD'], 'POST') != 0){
    $answer->Executed = false;
    $answer->Result = "The Request must method must be post";
}
else
{
    $contentType = isset($_SERVER["CONTENT_TYPE"]) ? trim($_SERVER["CONTENT_TYPE"]) : '';
    if (strcasecmp($contentType, 'application/json') != 0) {

        $answer->Executed = false;
        $answer->Result = "The content type must be like this : application/json";
    }
    else{
        //Receive the RAW post data.
        $content = trim(file_get_contents("php://input"));

//Attempt to decode the incoming RAW post data from JSON.
        $loginUser = json_decode($content, true);
        if (!is_array($loginUser)) {
            $answer->Executed = false;
            $answer->Result = "The Data you sent have bad format";

        }
        else  if(!isset($loginUser["Email"]) || empty($loginUser["Email"]) || !isset($loginUser["Password"]) || empty($loginUser["Password"])||!isset($loginUser["Username"]) || empty($loginUser["Username"])) {
            $answer->Executed = false;
            $answer->Result = "One or more parameters are in bad format";

        }else
        {
            $userData = new UserManager();
            $answer->Executed = true;
            $user = new User();
            //  $user->Id = $loginUser["Id"];
            // $user->Username = $loginUser["Username"];
            $user->Email = $loginUser["Email"];
            $user->Password = $loginUser["Password"];
            $user->Username = $loginUser["Username"];
            //  $answer->Result = $loginUser;

            $result = $userData->AddUSer($user);


            $loginResult = new RegisterResult();

            $loginResult->RegisteredIn = $result;
            if ($result)
                $loginResult->User = $user;
            else  $loginResult->User = null;
            $answer->Result = $loginResult;
        }


    }
}
$answer = json_encode($answer );
echo $answer;