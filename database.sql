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
  PRIMARY KEY (`application_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `application`
--

LOCK TABLES `application` WRITE;
/*!40000 ALTER TABLE `application` DISABLE KEYS */;
INSERT INTO `application` VALUES (0,'generic'),(1,'window media player'),(2,'notepad'),(3,'mozilla firefox'),(4,'microsoft word');
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
INSERT INTO `command` VALUES (1,'new',0,'^n'),(2,'open',0,'^o'),(3,'save',0,'^s'),(4,'delete',0,'1'),(5,'cut',0,'^x'),(6,'copy',0,'^c'),(7,'paste',0,'^v'),(8,'save as',0,'^+s'),(9,'rename',0,'{F2}'),(10,'undo',0,'^z'),(11,'redo',0,'^y'),(12,'search',0,'^f'),(13,'print',0,'^p'),(14,'tab',0,'{TAB}'),(15,'up',0,'{UP}'),(16,'down',0,'{DOWN}'),(17,'left',0,'{LEFT}'),(18,'right',0,'{RIGHT}'),(19,'click',0,'1'),(20,'double click',0,'1'),(21,'maximize',0,'maximize'),(22,'minimize',0,'minimize'),(23,'close',0,'%{F4}'),(24,'properties',0,'%~'),(25,'open with',0,NULL),(26,'send to',0,NULL),(27,'share with',0,NULL),(28,'cmd',0,'1'),(29,'move',0,'1'),(30,'task manager',0,'1'),(31,'refresh',0,'{F5}'),(32,'shutdown',0,'shutdown'),(33,'log off',0,'logoff'),(34,'enter',0,'~'),(35,'alt tab',0,'%{TAB}'),(36,'start',0,'1'),(37,'restore',0,'1'),(38,'select all',0,'^a'),(39,'video size 50%',1,'%1'),(40,'video size 100%',1,'%2'),(41,'video size 200%',1,'%3'),(42,'toggle display',1,'%{ENTER}'),(43,'retrace backwards',1,'%{LEFT}'),(44,'retrace forwards',1,'%{RIGHT}'),(45,'player library ',1,'^1'),(46,'skin mode',1,'^2'),(47,'now playing mode',1,'^3'),(48,'add to playlist',1,'^7'),(49,'add to burn list',1,'^8'),(50,'add to sync list',1,'^9'),(51,'select all',1,'^a'),(52,'previous item',1,'^b'),(53,'search',1,'^e'),(54,'next item',1,'^f'),(55,'shuffle',1,'^h'),(56,'eject',1,'^j'),(57,'menu bar',1,'^m'),(58,'open',1,'^o'),(59,'create new playlist',1,'^n'),(60,'play',1,'^p'),(61,'stop',1,'^s'),(62,'repeat',1,'^t'),(63,'URL',1,'^u'),(64,'close',1,'^w'),(65,'previous playlist',1,'^{LEFT}'),(66,'next playlist',1,'^{RIGHT}'),(67,'restart',1,'^B'),(68,'subtitles',1,'^C'),(69,'fast forward',1,'^F'),(70,'fast play speed',1,'^G'),(71,'normal speed',1,'^N'),(73,'help',1,'{F1}'),(74,'mute',1,'{F7}'),(75,'increase volume',1,'{F9}'),(76,'decrease volume',1,'{F8}'),(77,'file',2,'%f'),(78,'new',2,'^n'),(79,'open',2,'^o'),(80,'save',2,'^s'),(81,'save as',2,'%fa'),(82,'page setup',2,'%fu'),(83,'print',2,'^p'),(84,'close',2,'%fx'),(85,'edit menu',2,'%e'),(86,'undo',2,'^z'),(87,'copy ',2,'^c'),(88,'paste',2,'^v'),(89,'cut',2,'^x'),(90,'delete',2,'%el'),(91,'find',2,'^f'),(92,'find next',2,'{F3}'),(93,'replace',2,'^h'),(94,'go to',2,'^g'),(95,'select all',2,'^a'),(96,'time date',2,'{F5}'),(97,'format menu',2,'%o'),(98,'word wrap',2,'%ow'),(99,'font',2,'%of'),(100,'view menu',2,'%v'),(101,'status bar',2,'%vs'),(102,'help menu',2,'%h'),(103,'help',2,'{F1}'),(104,'help topic',2,'%hh'),(105,'about',2,'%ha'),(106,'private',3,'^p'),(107,'close',4,'%{F4}');
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
  `keyword` varchar(45) DEFAULT NULL,
  KEY `command_idx` (`command_id`),
  CONSTRAINT `command` FOREIGN KEY (`command_id`) REFERENCES `command` (`command_id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `synonym`
--

LOCK TABLES `synonym` WRITE;
/*!40000 ALTER TABLE `synonym` DISABLE KEYS */;
INSERT INTO `synonym` VALUES (1,'create'),(2,'explore'),(19,'select'),(31,'f5'),(9,'f2'),(36,'windows'),(12,'find'),(19,'choose'),(4,'remove'),(35,'start tab'),(28,'command prompt'),(15,'move up'),(18,'move right'),(16,'move down'),(17,'move left'),(9,'change name'),(5,'move'),(31,'reload'),(32,'turn off');
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

-- Dump completed on 2015-01-30  3:59:41
