<?php
/**
 * Created by PhpStorm.
 * User: Brandon Njimefo
 * Date: 26.05.2018
 * Time: 23:34
 */

class Token
{
public $Value;
public $ExpireDate;


    public function __construct()
    {
        $this->setValue();
    }
    function setValue()
    {
date_default_timezone_set("UTC");
        $dt = new DateTime();
        //echo $dt->format('Y-m-d H:i:s');
        $dt->add(new DateInterval("PT30M"));
$this->ExpireDate =$dt;
        $source = file_get_contents('/dev/urandom', false, null, null, 64);
        $source .= uniqid(uniqid(mt_rand(0, PHP_INT_MAX), true), true);
        for ($t=0; $t<64; $t++) {
            $source .= chr((mt_rand() ^ mt_rand()) % 256);
        }
        $this->Value = sha1(hash('sha512', $source, true));
    }


}