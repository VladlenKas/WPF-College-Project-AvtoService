-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: avtoservice_3curs_aa
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employee` (
  `id_employee` int NOT NULL AUTO_INCREMENT,
  `id_role` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `firstname` varchar(45) NOT NULL,
  `patronymic` varchar(45) DEFAULT NULL,
  `birthday` date NOT NULL,
  `seniority` int NOT NULL,
  `login` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  `passport` varchar(10) NOT NULL,
  `phone` varchar(11) NOT NULL,
  `is_deleted` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id_employee`),
  KEY `id_role_idx` (`id_role`),
  CONSTRAINT `id_role` FOREIGN KEY (`id_role`) REFERENCES `role` (`id_role`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
INSERT INTO `employee` VALUES (3,2,'Михаил','Старославянов','','1993-11-11',12,'misha','misha','8675676783','85654354354',1),(4,1,'Владлен','Касимов','Ильшатович','2006-08-01',0,'aaa','aaa','8020089211','89377888090',0),(6,1,'Мария','Колыванова','Ивановна','1993-10-03',2,'maria','maria','8745675432','85247632357',0),(21,3,'Арина','Московская','Никитовна','2001-01-12',1,'asd','asd','8764686566','80734798766',1),(26,3,'Касимов','Ленар','Ильшатович','2000-10-02',6,'fff','fff','3463655462','80364356346',0),(27,2,'Тимерлан','Касимов','Ильшатович','2000-08-25',5,'jjj','jjj','7435353635','86755473645',0),(29,1,'йцу','йцу','','2000-12-12',2,'123','12','1212121211','81212121211',0),(32,1,'йцу','йцу','йцу','2000-12-12',2,'12','12','1212121212','81212121212',0),(33,1,'йцу','йцу','йцу','2000-12-12',2,'11','12','1212121211','81212121211',1),(34,2,'Владлен','Касимов','Ильшатович','2000-12-12',1,'kasimovvladlen2006@gmail.comkasimovvladlen200','kasimovvladlen2006@gmail.comkasimovvladlen200','1212124543','85432452535',1),(35,2,'йцуке','цук','кен','2000-09-12',2,'qwert','qwqertr','6781234567','82345312343',0);
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-29  5:51:27
