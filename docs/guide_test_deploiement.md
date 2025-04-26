# Guide de test et déploiement d'IRIS-Forensic X

Ce guide détaille les procédures de test et de déploiement d'IRIS-Forensic X dans un environnement d'entreprise.

## Table des matières

1. [Environnement de test](#environnement-de-test)
2. [Tests fonctionnels](#tests-fonctionnels)
3. [Tests de performance](#tests-de-performance)
4. [Tests de sécurité](#tests-de-sécurité)
5. [Déploiement en entreprise](#déploiement-en-entreprise)
6. [Maintenance et mises à jour](#maintenance-et-mises-à-jour)
7. [Résolution des problèmes de déploiement](#résolution-des-problèmes-de-déploiement)

## Environnement de test

### Configuration recommandée

Pour tester efficacement IRIS-Forensic X avant un déploiement en production, nous recommandons la configuration suivante :

- **Système d'exploitation** : Windows 10/11 Professionnel ou Entreprise (64 bits)
- **Processeur** : Intel Core i7 ou équivalent (8 cœurs recommandés)
- **Mémoire** : 16 Go RAM minimum (32 Go recommandés)
- **Espace disque** : 100 Go minimum (SSD recommandé)
- **Résolution d'écran** : 1920x1080 ou supérieure
- **Connexion Internet** : 100 Mbps minimum

### Préparation de l'environnement

1. Créez une machine virtuelle dédiée aux tests (recommandé pour isoler l'environnement)
2. Installez le système d'exploitation Windows 10/11
3. Appliquez toutes les mises à jour disponibles
4. Installez les prérequis suivants :
   - Python 3.10 ou supérieur
   - Visual C++ Redistributable 2019
   - .NET Framework 4.8

## Tests fonctionnels

### Test d'installation

1. Téléchargez le package d'installation d'IRIS-Forensic X
2. Exécutez l'installateur MSI et vérifiez que l'installation se déroule sans erreur
3. Vérifiez que tous les composants sont correctement installés :
   - Application principale
   - Dépendances Python (notamment PyQt5)
   - Règles YARA par défaut
   - Modèles d'IA

### Test des fonctionnalités de base

#### Test de l'interface utilisateur

1. Lancez IRIS-Forensic X depuis le menu Démarrer
2. Vérifiez que l'interface se charge correctement
3. Testez la navigation entre les différents modules
4. Vérifiez que tous les boutons et contrôles fonctionnent correctement

#### Test de capture de données

1. Préparez des échantillons de test :
   - Un fichier VMDK de test (< 1 Go pour les tests rapides)
   - Des fichiers logs de différents formats
   - Des fichiers CSV contenant des données de test
2. Accédez au module Capture
3. Testez la capture de chaque type de fichier
4. Vérifiez que les données sont correctement extraites et stockées

#### Test d'analyse

1. Utilisez les données capturées lors du test précédent
2. Accédez au module Analyse
3. Testez l'analyse de chaque type de données
4. Vérifiez que les résultats d'analyse sont cohérents et accessibles

#### Test de génération de rapports

1. Utilisez les résultats d'analyse du test précédent
2. Accédez au module Rapports
3. Générez un rapport HTML
4. Vérifiez que le rapport contient toutes les informations attendues
5. Vérifiez que le rapport est correctement formaté et lisible

### Test des fonctionnalités avancées

#### Test d'analyse VMDK

1. Préparez un fichier VMDK contenant des indicateurs de compromission simulés
2. Accédez au module Analyse
3. Configurez une analyse complète avec toutes les options activées
4. Vérifiez que les menaces sont correctement détectées
5. Vérifiez que le score de menace est calculé correctement

#### Test des règles YARA

1. Accédez au module Règles YARA
2. Créez une nouvelle règle YARA personnalisée
3. Testez la règle sur un fichier contenant le pattern recherché
4. Vérifiez que la règle détecte correctement le pattern
5. Intégrez la règle dans une analyse complète et vérifiez qu'elle est appliquée

#### Test de l'intégration blockchain

1. Accédez au module Blockchain
2. Certifiez une preuve forensique
3. Vérifiez que la transaction blockchain est créée
4. Vérifiez l'intégrité de la preuve certifiée

## Tests de performance

### Test de charge

1. Préparez des fichiers VMDK de grande taille (> 10 Go)
2. Mesurez le temps nécessaire pour :
   - La capture des données
   - L'analyse complète
   - La génération de rapports
3. Surveillez l'utilisation des ressources système pendant les opérations
4. Documentez les résultats pour établir des références de performance

### Test de stabilité

1. Exécutez plusieurs analyses consécutives sur des fichiers volumineux
2. Laissez l'application fonctionner pendant au moins 24 heures
3. Vérifiez qu'aucune fuite de mémoire n'est détectée
4. Vérifiez que l'application reste stable et réactive

### Test de concurrence

1. Exécutez plusieurs instances d'IRIS-Forensic X simultanément
2. Lancez des analyses parallèles sur différents fichiers
3. Vérifiez que les performances restent acceptables
4. Vérifiez qu'aucun conflit n'apparaît entre les instances

## Tests de sécurité

### Vérification des privilèges

1. Installez IRIS-Forensic X avec différents niveaux de privilèges utilisateur
2. Vérifiez que l'application fonctionne correctement avec les privilèges minimaux requis
3. Identifiez les fonctionnalités nécessitant des privilèges élevés

### Test d'isolation

1. Vérifiez que l'analyse de fichiers malveillants est effectuée dans un environnement isolé
2. Confirmez qu'aucun code malveillant ne peut s'échapper pendant l'analyse
3. Vérifiez que les résultats d'analyse sont correctement sanitisés

### Audit de sécurité

1. Examinez les journaux d'application pour les informations sensibles
2. Vérifiez que les données sensibles sont correctement protégées
3. Assurez-vous que les communications réseau sont sécurisées

## Déploiement en entreprise

### Déploiement centralisé

#### Préparation du package de déploiement

1. Créez un package MSI personnalisé avec les options spécifiques à votre entreprise
2. Incluez un fichier de réponse pour l'installation silencieuse
3. Préparez un script post-installation pour configurer l'environnement

#### Déploiement via GPO (Group Policy Object)

1. Créez une nouvelle GPO dans votre domaine Active Directory
2. Configurez la GPO pour déployer le package MSI
3. Définissez les options d'installation silencieuse
4. Testez le déploiement sur un groupe pilote
5. Déployez progressivement sur l'ensemble du parc

#### Déploiement via SCCM (System Center Configuration Manager)

1. Importez le package MSI dans SCCM
2. Créez une application SCCM avec les paramètres appropriés
3. Définissez les conditions de déploiement et les exigences
4. Créez une collection pour les postes cibles
5. Déployez l'application sur la collection

### Configuration d'entreprise

#### Paramètres centralisés

1. Créez un fichier de configuration centralisé
2. Définissez les paramètres spécifiques à votre entreprise :
   - Emplacement des rapports
   - Règles YARA personnalisées
   - Configuration de la blockchain
3. Déployez la configuration via GPO ou SCCM

#### Intégration SIEM

1. Configurez IRIS-Forensic X pour envoyer les alertes au SIEM de l'entreprise
2. Définissez les règles de corrélation dans le SIEM
3. Testez l'intégration avec des cas de test connus

## Maintenance et mises à jour

### Stratégie de mise à jour

1. Définissez une politique de mise à jour :
   - Mises à jour automatiques ou manuelles
   - Fenêtres de maintenance
   - Procédure de validation des mises à jour
2. Créez un environnement de test pour valider les mises à jour
3. Documentez la procédure de rollback en cas de problème

### Sauvegarde des données

1. Identifiez les données critiques à sauvegarder :
   - Cas forensiques
   - Rapports générés
   - Règles YARA personnalisées
   - Configuration de l'application
2. Configurez des sauvegardes régulières
3. Testez la procédure de restauration

## Résolution des problèmes de déploiement

### Problèmes courants et solutions

#### Échec de l'installation silencieuse

**Symptômes** : L'installation silencieuse échoue sans message d'erreur visible.

**Solutions** :
1. Vérifiez les logs d'installation dans `%TEMP%`
2. Assurez-vous que l'utilisateur a les droits administratifs
3. Vérifiez que tous les prérequis sont installés
4. Utilisez la commande `msiexec /i IRIS-Forensic-X-Setup.msi /l*v install.log` pour générer des logs détaillés

#### Problèmes avec PyQt5

**Symptômes** : L'application démarre mais l'interface ne s'affiche pas correctement.

**Solutions** :
1. Vérifiez que PyQt5 est correctement installé : `pip show PyQt5`
2. Réinstallez PyQt5 manuellement : `pip install --force-reinstall PyQt5==5.15.6`
3. Vérifiez les conflits de dépendances : `pip check`

#### Performances insuffisantes

**Symptômes** : L'analyse des fichiers VMDK est extrêmement lente.

**Solutions** :
1. Vérifiez les spécifications matérielles du poste
2. Augmentez la mémoire RAM disponible
3. Déplacez les fichiers temporaires sur un SSD
4. Ajustez les paramètres d'analyse pour réduire la consommation de ressources

### Collecte d'informations de diagnostic

En cas de problème persistant, collectez les informations suivantes pour le support technique :

1. Logs d'application : `%APPDATA%\IRIS-Forensic-X\Logs\*.log`
2. Logs d'installation : `%TEMP%\IRIS-Forensic-X-*.log`
3. Configuration système : Exécutez `systeminfo > sysinfo.txt`
4. Liste des dépendances Python : Exécutez `pip freeze > pip_packages.txt`
5. Capture d'écran de l'erreur

Envoyez ces informations à support@iris-forensic-x.com en précisant :
- La version d'IRIS-Forensic X
- La description précise du problème
- Les étapes pour reproduire le problème
- Les actions déjà tentées pour résoudre le problème
