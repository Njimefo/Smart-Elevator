
//Bbliotheken einbinden
//Keypad für Handling von der Folientastatur
//ArduinoJson für Handling von Json Dateien
#include "ArduinoJson.h"
#include <Keypad.h>

//Festlegung der Etagenabstände
#define Etage0 78
#define Etage1 55
#define Etage2 26
#define Etage3 3


/************************/
//verschiedene Pins

//Pin zum Kontrollieren eines Schrittes des Schrittmotors
const int stepPin =11 ;
//Pin zum Steuern der Bewegungsrichtung des Schrittmotors
const int dirPin= 10;
//Pin zum Steuern der Befestigung der Welle des Motors
//Damit kann man zb. festlegen dass sich die Welle des Motors nicht mehr bewegen darf
const int enPin= 9;




//Erster Bewegungsmelder Pin
int motionPin1 = 6;

//Triger Pin des Ultraschall Sensors
int US_TrigerPin = 50;

//Echo Pin des Ultraschall Sensors
int US_EchoPin = 52;

//Ergebnis nach Berechnung der tatsÃ¤chlichen LÃ¤nge
long US_Ergebnis = 0;

//Gemessener Wert
long US_Messung = 0;



//----->>>>>>  Tastatur Folie  <<<<<<<<<<<<<<--------//

const byte ROWS = 4; // Anzahl der Zeilen der Tastatur Folie
const byte COLS = 3; // Anzahl der Spalten der Tastatur Folie

//Definiert das Tastaturschema
//Hier wird festgelegt, dass alles was nicht 1,2,3 oder 0 ist, soll - sein
char keys[ROWS][COLS] = {
  {
    '1', '2', '3'          }
  ,
  {
    '-', '-', '-'          }
  ,
  {
    '-', '-', '-'          }
  ,
  {
    '-', '0', '-'          }
};
 // verbindet die Zeilen 1, 2 , 3 und 4 jeweils an den Pins 8,7, 6 und 5
byte rowPins[ROWS] = { 
  8, 7, 6, 5 };
// verbindet die Spalten 1, 2  und 3 jeweils an den Pins 4,3 und 2
byte colPins[COLS] = { 
  4, 3, 2 };

// Erzeugt Keypad Objekt und es wird mit den verschiedenen Pins der Tastaturfolie und deren Spalten und Zeilen initialisiert
Keypad kpd = Keypad( makeKeymap(keys), rowPins, colPins, ROWS, COLS );
//----------------------------------------//


/**************************************/


/***********************************/
//verwendete Variablen
//Variable zum Steuern des Schrittes des Motors
const int step = 600;
int counter = 0;
//bestimmt, bei welcher Etage der Fahrstuhl sich gerade befindet
int actualEtage=0;





void setup() {
  
  //Setpu des Ultraschall Sensors
  US_Setup();
  //Setup des Motors
  MotorSetup();
  //Setup des 1.Bewegungssensors
  pinMode(motionPin1, INPUT);
  //Starten der seriellen Kommunikation
  Serial.begin(9600);
}




void loop() {
  
  //Bestimmung der aktuellen Etage
  determineEtage();
  //Liest die Tastatureingabe und bewegt den Motor dementsprechend
  Get_KeyboardInput();
  
  try 
  {
    //Versucht eventuelle Daten vom Server über serielle Schnittstelle auslesen
      int positionFromServer = (int)Serial.read();
      //bewegt den Motor zu der gewollten Position
  moveMotorToEtage(positionFromServer);

  }
  catch(...)
  {
  }
  
  
  //Einfache Datenverpackung zum Senden an den Server via den Raspberry Pi
  String data = String(US_Ergebnis);
  data +=";"+String(GetMotionStatus(motionPin1));
  data +=";"+String(actualEtage)+"\n";
 
 //alternative Datenverpackung im Json Format
 /* StaticJsonBuffer<200> jsonBuffer;
  JsonObject& root = jsonBuffer.createObject();
  AddValueToRoot("Position",US_Ergebnis,root);
  AddValueToRoot("Motion1",GetMotionStatus(motionPin1),root);
  AddValueToRoot("ActualEtage",actualEtage,root);
   root.printTo(Serial);*/
   
   //Senden der Daten via den seriellen Bus
   Serial.println(data);
   //Delay für den Empfänger zum Auslesen der Daten bevor weitere geschickt werden
   delay(500);
   

}

//Bestimmt die aktuelle Etage
void determineEtage()
{
  //Misst den Abstand zwischen dem Ultraschall Sensor und dem Kasten des fahrstuls
  US_Messen();

//Jenachdem welche Messwerte rauskommen, wird die aktuelle Etage bestimmt
  if(US_Ergebnis >= (int)Etage0)
  {
    actualEtage = 0; 
    
  }
  else if(US_Ergebnis < (int)Etage0 && US_Ergebnis >= (int)Etage1)
  {
    actualEtage = 1; 
    
  }
  else if(US_Ergebnis < (int)Etage1 && US_Ergebnis >= (int)Etage2)
  {
    actualEtage = 2; 
    
  }
  else 
  {
    actualEtage = 3; 
    
  }
}

//Setup des Ultraschall Sensors
void US_Setup()
{
  pinMode(US_TrigerPin, OUTPUT);
  pinMode(US_EchoPin, INPUT);
}

//Setup des Motors

void MotorSetup()
{
  pinMode(stepPin, OUTPUT);
  pinMode(dirPin, OUTPUT);

  pinMode(enPin, OUTPUT);
  digitalWrite(enPin, LOW);
}


//Bewegt den Motor von einem kurzen Schritt
void moveMotor()
{
  //Geschwindigkeit des Bewegung
  int geschwindigkeit = 50; 

 
 //Da es sich um einen Schrittmotor geht, wird der Motor mit kleinen Shritten bewegt
  for (int x = 0; x < step; x++) {
    digitalWrite(stepPin, HIGH);
    delayMicroseconds(geschwindigkeit);
    digitalWrite(stepPin, LOW);
    delayMicroseconds(geschwindigkeit);
    // digitalWrite(enPin, LOW);
  }

}
//Bewegt den Motor zu einer bestimmten Etage
void moveMotorToEtage(int pos)
{
  //Gibt an, bis zum Abstand der Fahstuhl sich bewegen soll
  int toEtage= 0;
  //bestimme die Aktuelle Etage
  determineEtage();   
  
  //bestimmt, bis zum Abstand der Fahstuhl sich bewegen soll
  if(pos==0)
  toEtage = Etage0;
  else if( pos==1)
  toEtage = Etage1;
  else if (pos == 2)
  toEtage = Etage2;
  else toEtage = Etage3;
  
  //Bewegt den Motor bis zum erwarteten Abstand
  moveMotorToPosition(toEtage);

}

//bewegt den Motor bis zu einem gegebene Abstand
void moveMotorToPosition(int pos)
{
  //bestimmt ob die Bewegung aufwaärts oder abwärts ist
  bool forward = true;
  
  //Misst die aktuelle Position des fahrstuhls
  US_Messen();

//wenn sich der Fahrtuhl sich schon in der gewollten Position befindet dann darf gar nichts mehr gemacht werden
  if(pos==US_Ergebnis)
    return;
  else if(pos<US_Ergebnis)
    forward = false;
  else forward =true;
  

//Setzen des Pin Stand je nachdem welche Bewegungsrichtung erwartet wird
  if(forward)
    digitalWrite(dirPin, HIGH);
  else digitalWrite(dirPin, LOW);
  
  //gibt an, ob der erwartete Abstand schon erreicht wurde
  boolean reached = false;
  
  //Bewegt den Motor solange den Abstand nicht erreicht wurde
  do
  {
    moveMotor();
    US_Messen();
    reached = (forward && pos<=US_Ergebnis)||(!forward && pos>=US_Ergebnis);
  }
  while(!reached);
}


//Misst den Abstand
void US_Messen()
{
 
  digitalWrite(US_TrigerPin, LOW);
  //2 Millisekunden abwarten
  delayMicroseconds(2);
  //Triger(Messinstrument) einschalten
  digitalWrite(US_TrigerPin, HIGH);
  //10 Millisekunden abwarten
  delayMicroseconds(10);
  //Triger(Messinstrument) ausschalten
  digitalWrite(US_TrigerPin, LOW);
  //gemessenen Wert holen
  US_Messung = pulseIn(US_EchoPin, HIGH);
  US_Ergebnis = (US_Messung / 2) / 29;
  /**/


}

//Holt die Tastatureingabe und bewegt den Motor dementsprechend
void Get_KeyboardInput()
{
  char key = kpd.getKey();

  if (key == '0' || key == '1' || key == '2' || key == '3')
  {
    Serial.println(key);
    
    if(key=='0') moveMotorToEtage(0);
    else if (key=='1') moveMotorToEtage(1); 
    else if (key=='2') moveMotorToEtage(2); 
    else moveMotorToEtage(3); 

  }
}

//Get Bewegungsmelder Zustand
bool GetMotionStatus(int motionPin)
{
  int motionStatus = digitalRead(motionPin);
  if (motionStatus == HIGH)
  {
    return true;
  }
  else
  {
    return false;
  }
}

//Fügt einen String Wert in dem Json Hauptelement 
void AddValueToRoot(String dataName, String data, JsonObject& root)
{
  root[dataName] = data;
}

//Fügt einen long Wert in dem Json Hauptelement
void AddValueToRoot(String dataName, long data, JsonObject& root)
{
  root[dataName] = data;
}

//Fügt einen bool Wert in dem Json Hauptelement
void AddValueToRoot(String dataName, bool data, JsonObject& root)
{
  root[dataName] = data;
}

//Fügt einen int Wert in dem Json Hauptelement
void AddValueToRoot(String dataName, int data, JsonObject& root)
{
  root[dataName] = data;
}

//Fügt einen double Wert in dem Json Hauptelement
void AddValueToRoot(String dataName, double data, JsonObject& root)
{
  root[dataName] = data;
}




