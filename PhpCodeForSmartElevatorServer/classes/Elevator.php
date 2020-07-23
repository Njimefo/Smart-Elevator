<?php
class Lift
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
 $this->ActualEtage = -1;
 $this->GoTo = -1;
 $this->Distance=-1;
 $this->MotionDetector0 = false;
 $this->MotionDetector1 = false;
 $this->MotionDetector2 = false;
 $this->MotionDetector3 = false;
}

}
