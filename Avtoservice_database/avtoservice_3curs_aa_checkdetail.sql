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
-- Table structure for table `checkdetail`
--

DROP TABLE IF EXISTS `checkdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `checkdetail` (
  `id_checkdetail` int NOT NULL AUTO_INCREMENT,
  `id_sale` int NOT NULL,
  `id_detail` int NOT NULL,
  `details_count` int NOT NULL,
  PRIMARY KEY (`id_checkdetail`),
  KEY `id_detail_idx` (`id_detail`),
  KEY `id_sale_idx` (`id_sale`),
  CONSTRAINT `fk_checkdetail_id_detail` FOREIGN KEY (`id_detail`) REFERENCES `detail` (`id_detail`),
  CONSTRAINT `fk_checkdetail_id_sale` FOREIGN KEY (`id_sale`) REFERENCES `sale` (`id_sale`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `checkdetail`
--

LOCK TABLES `checkdetail` WRITE;
/*!40000 ALTER TABLE `checkdetail` DISABLE KEYS */;
INSERT INTO `checkdetail` VALUES (14,12,5,1),(15,14,7,19),(16,16,1,3),(17,16,6,5),(18,16,5,2),(19,16,8,4),(20,29,4,7),(21,29,3,5);
/*!40000 ALTER TABLE `checkdetail` ENABLE KEYS */;
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
