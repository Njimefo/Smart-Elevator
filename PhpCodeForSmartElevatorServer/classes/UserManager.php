<?php
/**
 * Created by PhpStorm.
 * User: Brandon Njimefo
 * Date: 26.05.2018
 * Time: 22:45
 */
include_once 'User.php';
class UserManager
{

    public $Users = array();

    /**
     * UserManager constructor.
     */
    public function __construct()
    {
        $this->ReadAllData();
    }
 function LoadUsers()
 {

 }
 function AddUSer(User $user)
 {
     try {

         foreach ((array)$this->Users as $value) {
if($value!=null)
if($value->Username==$user->Username)            
return false;
        }

         $this->Users[] = $user;
         $this->WriteAllData();
         return true;

     } catch (Exception $e) {
         echo 'Caught exception: ',  $e->getMessage(), "\n";
         return false;
     }

 }

 public function UpdateUserGoto($username,$goto)
    {
   $counter = 0;
foreach ((array)$this->Users as $user)
        {
if($user->Username== $username)
{
 $this->Users[$counter]->ElevatorN->GoTo = $goto;
   $this->WriteAllData();
            break;

}
   $counter +=1;
}
}


 public function UpdateElevator($username,$elevator)
    {
   $counter = 0;
foreach ((array)$this->Users as $user)
        {
if($user->Username== $username)
{
 $this->Users[$counter]->ElevatorN = $elevator;
   $this->WriteAllData();
            break;

}
   $counter +=1;
}
}



    public function GetUser($username,$setToken=true)
    {
        $counter = 0;
        foreach ((array)$this->Users as $user)
        {
            if($user->Username==$username)
            {
  if ($setToken)
                                if($user->CurrentToken!=null&&$user->CurrentToken->ExpireDate!=null)
                {

date_default_timezone_set("UTC");
                    $dt = new DateTime();
                    $interval = date_diff($dt,$user->CurrentToken->ExpireDate);

                   if($interval->i<1&&$interval->s<1)
                       $this->Users[$counter]->setToken();
                    }
                else
                    $this->Users[$counter]->setToken();
                $this->WriteAllData();
                return  $this->Users[$counter];
            }
            $counter +=1;
        }

        return null;

    }


    static  public function  CheckPassword(UserManager $data,User $user)
    {
        $foundAndCorrect = false;
        foreach ((array)$data->Users as $value)
        {
if($value!=null && $user!=null)
            if($value->Username == $user->Username&&$value->Password==$user->Password)
            {
                $foundAndCorrect = true;
                break;
            }

        }
        return $foundAndCorrect;
    }


    public function ReadAllData()
    {
        $path = "./Users.txt";
        $this->Users=  unserialize(file_get_contents($path));
        return $this->Users;
    }
    public function WriteAllData()
    {
        $path = "./Users.txt";
        return file_put_contents($path,serialize($this->Users));
    }




}