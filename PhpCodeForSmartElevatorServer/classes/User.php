<?php
/**
 * Created by PhpStorm.
 * User: Brandon Njimefo
 * Date: 26.05.2018
 * Time: 22:42
 */

include_once  'Reservation.php';
include_once 'Token.php';
include_once 'Elevator.php';
class User
{
    public $Username;
public $ElevatorN;
public $Commands = array();
public function __construct()
    {
 $this->ElevatorN = new Lift();


}
      public $CurrentToken;
    public function setToken()
    {
        $this->CurrentToken = new Token();
    }
    public $Password;
    public $Email;
}
class Elevator
{
public $ActualEtage;
public $GoTo;
public $Distance;
public $MotionDetector0;
public $MotionDetector1;
public $MotionDetector2;
public $MotionDetector3;
public function __construct()
    {
$ActualEtage = -1;
$GoTo = -1;
$Distance=-1;
$MotionDetector0 = false;
$MotionDetector1 = false;
$MotionDetector2 = false;
$MotionDetector3 = false;
}

}
class Command
{
public $Etage;
public $Time;


}
