# SRRMS -- Student Registry RFID Management System

***Coded by <a href="https://github.com/bradley-kyan" title="bradley-kyan Github">Kyan Bradley</a> -- bradley-kyan***

## Overview ##
SRRMS (Student Registry RFID Management System) is a project created to record student attenence through the use of RFID tags. The solution is designed to be cost effective and easy to set up and install.
### What is contained in this repository ###
The code required to setup and run the Arduino based card reader, the TCP listening server and the queries required to setup the database
## Dependencies and Requirements ##
To reporoduce this project the following are needed:
<ul>
  <li>Arduino Uno
  <li>MRFC-522 Shield
  <li>Ethernet Shield
  <li>Piezo Buzzer
  <li>Windows 10 running Visual Studio 2019 or later
  <li>Arduino IDE
  <li>Reliable internet connetion
  <li>SQL Server
</ul>

## Installation ##
### Arduino ###
Connect the shields to the Arduino Uno according to the sketch. Upload the sketch to the Arduino.

### Listening Server ###
Set your pc to have a static ip of 192.168.1.75, as the arduino sketch is set to send to that address @ port 29882. Can change these values in the sketch and in the ServerStartup.cs. Complie the code and run. First set up the connection strings, and verified device ids. The device id can be found in the Arduino sketch (value can be changed when uploading to the Arduino). Restart the application. Start the server.

### Database ###
Run the 'SRRMS Creation Simplified' query in SQL Server, then run the rest of the queries in any order.
