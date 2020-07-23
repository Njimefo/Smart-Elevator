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
/*if(strcasecmp($_SERVER['REQUEST_METHOD'], 'POST') != 0){
$answer->Executed = false;
$answer->Result = "The Request must method must be post";
}*/
if($_SERVER['REQUEST_METHOD'] != 'POST')
{
$answer->Executed = false;
$answer->Result = "The Request must method must be post";
$answer->Result = $_SERVER['REQUEST_METHOD'];
}
else
{
    $contentType = isset($_SERVER["CONTENT_TYPE"]) ? trim($_SERVER["CONTENT_TYPE"]) : '';
    if ($contentType!='application/json' && $contentType != 'application/json; charset=utf-8' ) {

        $answer->Executed = false;
  $answer->Result = "The content type must be like this : application/json";
//$answer->Result = $contentType;
    }
    else{
        //Receive the RAW post data.
        $content = file_get_contents('php://input');

//Attempt to decode the incoming RAW post data from JSON.

       $loginUser = json_decode($content, true);
        if (!is_array($loginUser)) {
            $answer->Executed = false;
      $answer->Result = "The Data you sent have bad format";
         }
        else  if(!isset($loginUser["Username"]) || empty($loginUser["Username"]) || !isset($loginUser["Password"]) || empty($loginUser["Password"])) {
            $answer->Executed = false;
            $answer->Result = "One or more parameters are in bad format";

        }else
        {

            $userData = new UserManager();
            $answer->Executed = true;
            $user = new User();
                        $user->Username = $loginUser["Username"];
          $user->Password = $loginUser["Password"];
            

            $result = UserManager::CheckPassword($userData, $user);


            $loginResult = new LoggingResult();

            $loginResult->LoggedIn = $result;
            if ($result)
                $loginResult->User = $userData->GetUser($user->Username);
            else  $loginResult->User = null;
            $answer->Result = $loginResult;
     }


    }
}
$answer = json_encode($answer );
echo $answer;
?>
