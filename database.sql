CREATE DATABASE  IF NOT EXISTS `voice` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `voice`;
-- MySQL dump 10.13  Distrib 5.6.17, for Win32 (x86)
--
-- Host: 127.0.0.1    Database: voice
-- ------------------------------------------------------
-- Server version	5.5.1-m2-community

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `application`
--

DROP TABLE IF EXISTS `application`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `application` (
  `application_id` int(11) NOT NULL,
  `application_name` varchar(45) DEFAULT NULL,
  `path` varchar(90) DEFAULT NULL,
  PRIMARY KEY (`application_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `application`
--

LOCK TABLES `application` WRITE;
/*!40000 ALTER TABLE `application` DISABLE KEYS */;
INSERT INTO `application` VALUES (0,'generic',NULL),(1,'windows media player','wmplayer.exe'),(2,'notepad','notepad.exe'),(3,'mozilla firefox','firefox.exe'),(4,'microsoft word','winword.exe'),(5,'custom commands',NULL);
/*!40000 ALTER TABLE `application` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `command`
--

DROP TABLE IF EXISTS `command`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `command` (
  `command_id` int(11) NOT NULL,
  `command_name` varchar(20) DEFAULT NULL,
  `application_id` int(11) DEFAULT NULL,
  `action` varchar(45) DEFAULT NULL,
  `shortcut` varchar(45) DEFAULT NULL,
  `status` int(11) DEFAULT NULL,
  PRIMARY KEY (`command_id`),
  KEY `app_idx` (`command_name`),
  KEY `app_idx1` (`application_id`),
  CONSTRAINT `app` FOREIGN KEY (`application_id`) REFERENCES `application` (`application_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `command`
--

LOCK TABLES `command` WRITE;
/*!40000 ALTER TABLE `command` DISABLE KEYS */;
INSERT INTO `command` VALUES (1,'new',0,'^n','CTRL+N',NULL),(2,'open',0,'^o','CTRL+O',NULL),(3,'save',0,'^s','CTRL+S',NULL),(4,'delete',0,'1',NULL,NULL),(5,'cut',0,'^x','CTRL+X',NULL),(6,'copy',0,'^c','CTRL+C',NULL),(7,'paste',0,'^v','CTRL+V',NULL),(8,'save as',0,'^+s','CTRL+SHIFT+S',NULL),(9,'rename',0,'{F2}','F2',NULL),(10,'undo',0,'^z','CTRL+Z',NULL),(11,'redo',0,'^y','CTRL+Y',NULL),(12,'search',0,'^f','CTRL+F',NULL),(13,'print',0,'^p','CTRL+P',NULL),(14,'tab',0,'{TAB}','TAB',NULL),(15,'up',0,'{UP}','UP',NULL),(16,'down',0,'{DOWN}','DOWN',NULL),(17,'left',0,'{LEFT}','LEFT',NULL),(18,'right',0,'{RIGHT}','RIGHT',NULL),(19,'click',0,'1',NULL,NULL),(20,'double click',0,'1',NULL,NULL),(21,'maximize',0,'.maximize',NULL,NULL),(22,'minimize',0,'.minimize',NULL,NULL),(23,'close',0,'%{F4}','ALT+F4',NULL),(24,'properties',0,'%~','ALT+ENTER',NULL),(25,'open with',0,NULL,NULL,NULL),(26,'send to',0,NULL,NULL,NULL),(27,'share with',0,NULL,NULL,NULL),(28,'cmd',0,'1',NULL,NULL),(29,'move',0,'1',NULL,NULL),(30,'task manager',0,'1',NULL,NULL),(31,'refresh',0,'{F5}','F5',NULL),(32,'shutdown',0,'.shutdown',NULL,NULL),(33,'log off',0,'.logoff',NULL,NULL),(34,'enter',0,'~','ENTER',NULL),(35,'alt tab',0,'%{TAB}','',NULL),(36,'start',0,'1',NULL,NULL),(37,'restore',0,'1',NULL,NULL),(38,'select all',0,'^a','CTRL+A',NULL),(39,'video size 50%',1,'%1','ALT+1',0),(40,'video size 100%',1,'%2','ALT+2',0),(41,'video size 200%',1,'%3','ALT+3',0),(42,'toggle display',1,'%{ENTER}','ALT+ENTER',1),(43,'retrace backwards',1,'%{LEFT}','ALT+LEFT',NULL),(44,'retrace forwards',1,'%{RIGHT}','ALT+RIGHT',NULL),(45,'player library ',1,'^1','CTRL+1',1),(46,'skin mode',1,'^2','CTRL+2',1),(47,'now playing mode',1,'^3','CTRL+3',1),(48,'add to playlist',1,'^7','CTRL+7',NULL),(49,'add to burn list',1,'^8','CTRL+8',NULL),(50,'add to sync list',1,'^9','CTRL+9',NULL),(51,'select all',1,'^a','CTRL+A',1),(52,'previous item',1,'^b','CTRL+B',1),(53,'search',1,'^e','CTRL+E',1),(54,'next item',1,'^f','CTRL+F',1),(55,'shuffle',1,'^h','CTRL+H',1),(56,'eject',1,'^j','CTRL+J',0),(57,'menu bar',1,'^m','CTRL+M',1),(58,'open',1,'^o','CTRL+O',1),(59,'create new playlist',1,'^n','CTRL+N',1),(60,'play',1,'^p','CTRL+P',1),(61,'stop',1,'^s','CTRL+S',1),(62,'repeat',1,'^t','CTRL+T',1),(63,'URL',1,'^u','CTRL+U',1),(64,'close',1,'^w','CTRL+W',0),(65,'previous playlist',1,'^{LEFT}','CTRL+LEFT',1),(66,'next playlist',1,'^{RIGHT}','CTRL+RIGHT',1),(67,'fast backward',1,'^+b','CTRL+SHIFT+B',1),(68,'subtitles',1,'^+c','CTRL+SHIFT+C',NULL),(69,'fast forward',1,'^+f','CTRL+SHIFT+F',NULL),(70,'fast play speed',1,'^+g','CTRL+SHIFT+G',1),(71,'normal speed',1,'^+n','CTRL+SHIFT+N',1),(73,'help',1,'{F1}','F1',1),(74,'mute',1,'{F7}','F7',1),(75,'increase volume',1,'{F9}','F9',1),(76,'decrease volume',1,'{F8}','F8',1),(77,'file',2,'%f','ALT+F',NULL),(78,'new',2,'^n','CTRL+N',NULL),(79,'open',2,'^o','CTRL+O',NULL),(80,'save',2,'^s','CTRL+S',NULL),(81,'save as',2,'%fa','ALT+F+A',NULL),(82,'page setup',2,'%fu','ALT+F+U',NULL),(83,'print',2,'^p','CTRL+P',NULL),(84,'close',2,'%fx','ALT+F+X',NULL),(85,'edit menu',2,'%e','ALT+E',NULL),(86,'undo',2,'^z','CTRL+Z',NULL),(87,'copy ',2,'^c','CTRL+C',NULL),(88,'paste',2,'^v','CTRL+V',NULL),(89,'cut',2,'^x','CTRL+X',NULL),(90,'delete',2,'%el','ALT+E+L',NULL),(91,'find',2,'^f','CTRL+F',NULL),(92,'find next',2,'{F3}','F3',NULL),(93,'replace',2,'^h','CTRL+H',NULL),(94,'go to',2,'^g','CTRL+G',NULL),(95,'select all',2,'^a','CTRL+A',NULL),(96,'time date',2,'{F5}','F5',NULL),(97,'format menu',2,'%o','ALT+O',NULL),(98,'word wrap',2,'%ow','ALT+O+W',NULL),(99,'font',2,'%of','ALT+O+F',NULL),(100,'view menu',2,'%v','CTRL+V',NULL),(101,'status bar',2,'%vs','CTRL+V+S',NULL),(102,'help menu',2,'%h','CTRL+H',NULL),(103,'help',2,'{F1}','F1',NULL),(104,'help topic',2,'%hh','ALT+H+H',NULL),(105,'about',2,'%ha','ALT+H+A',NULL),(106,'private',3,'^p','CTRL+P',NULL),(107,'close',4,'%{F4}','ALT+F4',NULL),(108,'enter',0,'{ENTER}','ENTER',NULL),(109,'enter',1,'{ENTER}','ENTER',NULL),(110,'enter',2,'{ENTER}','ENTER',NULL),(111,'sleep',0,'.sleep',NULL,NULL),(112,'hibernate',0,'.hibernate',NULL,NULL),(113,'facebook',5,'http://www.facebook.com','http://www.facebook.com',NULL),(114,'Music',5,'D:\\Music','D:\\Music',NULL),(115,'uninstall',0,'.uninstall',NULL,NULL),(116,'rightclick',0,'+{F10}',NULL,NULL),(117,'rundo',0,'u',NULL,NULL),(118,'rpaste',0,'p',NULL,NULL),(119,'rdelete',0,'d',NULL,NULL),(120,'show unicode',0,'+{F10}s',NULL,NULL);
/*!40000 ALTER TABLE `command` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `synonym`
--

DROP TABLE IF EXISTS `synonym`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `synonym` (
  `command_id` int(11) NOT NULL,
  `keyword` varchar(45) NOT NULL,
  `s_id` varchar(45) NOT NULL,
  PRIMARY KEY (`s_id`,`keyword`),
  KEY `command_idx` (`command_id`),
  CONSTRAINT `command` FOREIGN KEY (`command_id`) REFERENCES `command` (`command_id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `synonym`
--

LOCK TABLES `synonym` WRITE;
/*!40000 ALTER TABLE `synonym` DISABLE KEYS */;
INSERT INTO `synonym` VALUES (1,'create','1'),(2,'explore','2'),(4,'remove','3'),(5,'move','4'),(9,'change name','5'),(9,'f2','6'),(12,'find','7'),(15,'move up','8'),(16,'move down','9'),(17,'move left','10'),(18,'move right','11'),(19,'choose','12'),(19,'select','13'),(28,'command prompt','14'),(31,'f5','15'),(31,'reload','16'),(32,'turn off','17'),(35,'start tab','18'),(36,'windows','19');
/*!40000 ALTER TABLE `synonym` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-03-01  1:55:29
