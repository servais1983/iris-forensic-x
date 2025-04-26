# Manuel d'utilisation d'IRIS-Forensic X

Ce manuel détaille les fonctionnalités et l'utilisation d'IRIS-Forensic X, un outil forensique hybride avancé pour l'analyse de cyberattaques.

## Table des matières

1. [Introduction](#introduction)
2. [Interface utilisateur](#interface-utilisateur)
3. [Capture de données](#capture-de-données)
4. [Analyse forensique](#analyse-forensique)
5. [Triage des cas](#triage-des-cas)
6. [Réponse aux incidents](#réponse-aux-incidents)
7. [Règles YARA](#règles-yara)
8. [Génération de rapports](#génération-de-rapports)
9. [Intégration blockchain](#intégration-blockchain)
10. [Fonctionnalités avancées](#fonctionnalités-avancées)
11. [Dépannage](#dépannage)

## Introduction

IRIS-Forensic X est un outil forensique hybride qui combine une interface CLI puissante, une interface graphique intuitive, des capacités d'IA avancées et une intégration blockchain pour l'analyse forensique complète. Conçu pour les analystes en cybersécurité, il permet d'analyser efficacement les fichiers VMDK, logs et CSV pour détecter des malwares, ransomwares, traces de phishing, backdoors et autres indicateurs de compromission.

### Principales fonctionnalités

- **Collecte intelligente "3-en-1"** : Capture de mémoire, logs et trafic réseau
- **Analyse unifiée IA/Classique** : Détection automatique des menaces avec validation humaine
- **Support multi-format** : Analyse de fichiers VMDK, logs, CSV et autres formats
- **Intégration de règles YARA** : Détection personnalisable de menaces
- **Système de scoring** : Évaluation automatique du niveau de menace
- **Génération de rapports HTML** : Documentation détaillée des résultats d'analyse
- **Certification blockchain** : Garantie d'intégrité des preuves forensiques

## Interface utilisateur

L'interface d'IRIS-Forensic X est organisée en modules accessibles depuis le menu de navigation latéral :

![Interface principale](../images/interface_principale.png)

### Menu de navigation

- **Dashboard** : Vue d'ensemble des cas et statistiques
- **Capture** : Outils de collecte de données forensiques
- **Analyse** : Fonctionnalités d'analyse des données collectées
- **Triage** : Classification et priorisation des cas
- **Réponse** : Outils de réponse aux incidents
- **Règles YARA** : Gestion des règles de détection
- **Rapports** : Génération et gestion des rapports
- **Blockchain** : Certification et vérification des preuves

## Capture de données

Le module Capture permet de collecter des données forensiques à partir de différentes sources.

### Capture de fichiers VMDK

1. Accédez à l'onglet **Capture** dans le menu de navigation
2. Cliquez sur le bouton **Nouvelle capture**
3. Sélectionnez **Fichier VMDK** comme type de source
4. Cliquez sur **Parcourir** et sélectionnez le fichier VMDK à analyser
5. Configurez les options de capture :
   - **Capture mémoire** : Extrait les données de la mémoire virtuelle
   - **Capture système de fichiers** : Analyse le système de fichiers
   - **Capture registre** : Extrait les données du registre (Windows)
6. Cliquez sur **Démarrer la capture**
7. Suivez la progression dans la barre d'état

### Capture de logs

1. Accédez à l'onglet **Capture** dans le menu de navigation
2. Cliquez sur le bouton **Nouvelle capture**
3. Sélectionnez **Fichiers logs** comme type de source
4. Cliquez sur **Parcourir** et sélectionnez le(s) fichier(s) log à analyser
5. Configurez les options de capture :
   - **Format de log** : Sélectionnez le format (Apache, IIS, Syslog, etc.)
   - **Fuseau horaire** : Définissez le fuseau horaire des logs
   - **Filtres** : Appliquez des filtres pour cibler certains événements
6. Cliquez sur **Démarrer la capture**
7. Suivez la progression dans la barre d'état

### Capture CSV

1. Accédez à l'onglet **Capture** dans le menu de navigation
2. Cliquez sur le bouton **Nouvelle capture**
3. Sélectionnez **Fichier CSV** comme type de source
4. Cliquez sur **Parcourir** et sélectionnez le fichier CSV à analyser
5. Configurez les options de capture :
   - **Délimiteur** : Spécifiez le délimiteur utilisé (virgule, point-virgule, etc.)
   - **En-têtes** : Indiquez si le fichier contient une ligne d'en-tête
   - **Colonnes d'intérêt** : Sélectionnez les colonnes à analyser
6. Cliquez sur **Démarrer la capture**
7. Suivez la progression dans la barre d'état

## Analyse forensique

Le module Analyse permet d'examiner en profondeur les données capturées pour détecter des menaces et des indicateurs de compromission.

### Analyse de fichiers VMDK

1. Accédez à l'onglet **Analyse** dans le menu de navigation
2. Sélectionnez la capture VMDK à analyser dans la liste
3. Cliquez sur **Nouvelle analyse**
4. Configurez les options d'analyse :
   - **Utiliser l'IA** : Active l'analyse par intelligence artificielle
   - **Utiliser YARA** : Active l'analyse par règles YARA
   - **Rechercher malwares** : Détecte les logiciels malveillants
   - **Rechercher ransomwares** : Détecte spécifiquement les ransomwares (LockBit 3.0, etc.)
   - **Rechercher phishing** : Détecte les indicateurs de phishing
   - **Rechercher backdoors** : Détecte les portes dérobées et persistance
5. Cliquez sur **Démarrer l'analyse**
6. Suivez la progression dans la barre d'état
7. Consultez les résultats dans l'onglet **Résultats**

### Analyse de logs

1. Accédez à l'onglet **Analyse** dans le menu de navigation
2. Sélectionnez la capture de logs à analyser dans la liste
3. Cliquez sur **Nouvelle analyse**
4. Configurez les options d'analyse :
   - **Détection d'anomalies** : Identifie les comportements anormaux
   - **Corrélation d'événements** : Établit des liens entre événements
   - **Recherche d'IOCs** : Détecte les indicateurs de compromission connus
5. Cliquez sur **Démarrer l'analyse**
6. Suivez la progression dans la barre d'état
7. Consultez les résultats dans l'onglet **Résultats**

### Analyse CSV

1. Accédez à l'onglet **Analyse** dans le menu de navigation
2. Sélectionnez la capture CSV à analyser dans la liste
3. Cliquez sur **Nouvelle analyse**
4. Configurez les options d'analyse :
   - **Type d'analyse** : Sélectionnez le type d'analyse adapté aux données
   - **Seuil d'alerte** : Définissez le seuil de détection d'anomalies
5. Cliquez sur **Démarrer l'analyse**
6. Suivez la progression dans la barre d'état
7. Consultez les résultats dans l'onglet **Résultats**

## Triage des cas

Le module Triage permet de classer et prioriser les cas forensiques en fonction de leur gravité et de leur impact potentiel.

### Triage automatique

1. Accédez à l'onglet **Triage** dans le menu de navigation
2. Cliquez sur **Triage automatique**
3. Sélectionnez le répertoire contenant les cas à trier
4. Configurez les options de triage :
   - **Critères de priorité** : Définissez les critères de priorisation
   - **Seuil critique** : Définissez le seuil pour les cas critiques
5. Cliquez sur **Démarrer le triage**
6. Consultez les résultats dans la liste des cas triés

### Triage manuel

1. Accédez à l'onglet **Triage** dans le menu de navigation
2. Sélectionnez un cas dans la liste
3. Examinez les détails du cas dans le panneau d'information
4. Attribuez une priorité au cas (Critique, Élevée, Moyenne, Faible)
5. Ajoutez des notes et des commentaires si nécessaire
6. Cliquez sur **Enregistrer** pour sauvegarder les modifications

## Réponse aux incidents

Le module Réponse permet de définir et d'exécuter des actions de remédiation suite à la détection d'incidents.

### Réponse automatique

1. Accédez à l'onglet **Réponse** dans le menu de navigation
2. Sélectionnez le cas à traiter dans la liste
3. Cliquez sur **Réponse automatique**
4. Sélectionnez la règle de réponse à appliquer (ex: GDPR_Breach)
5. Vérifiez les actions qui seront exécutées
6. Cliquez sur **Exécuter** pour lancer la réponse automatique
7. Suivez la progression dans la barre d'état
8. Consultez le rapport de réponse généré

### Réponse manuelle

1. Accédez à l'onglet **Réponse** dans le menu de navigation
2. Sélectionnez le cas à traiter dans la liste
3. Cliquez sur **Réponse manuelle**
4. Sélectionnez les actions à exécuter :
   - **Isolation** : Isole les systèmes compromis
   - **Collecte de preuves** : Collecte des preuves supplémentaires
   - **Notification** : Génère des notifications aux parties concernées
5. Cliquez sur **Exécuter** pour lancer les actions sélectionnées
6. Suivez la progression dans la barre d'état
7. Consultez le rapport de réponse généré

## Règles YARA

Le module Règles YARA permet de gérer les règles de détection utilisées pour l'analyse forensique.

### Gestion des règles YARA

1. Accédez à l'onglet **Règles YARA** dans le menu de navigation
2. Consultez la liste des règles disponibles
3. Pour activer/désactiver une règle, cochez/décochez la case correspondante
4. Pour modifier une règle, cliquez sur le bouton **Modifier**
5. Pour créer une nouvelle règle, cliquez sur **Nouvelle règle**

### Création d'une règle YARA

1. Accédez à l'onglet **Règles YARA** dans le menu de navigation
2. Cliquez sur **Nouvelle règle**
3. Remplissez les champs du formulaire :
   - **Nom** : Nom unique pour la règle
   - **Description** : Description de la menace détectée
   - **Auteur** : Votre nom ou identifiant
   - **Sévérité** : Niveau de gravité (1-5)
   - **Tags** : Mots-clés pour catégoriser la règle
4. Rédigez le contenu de la règle dans l'éditeur
5. Cliquez sur **Tester** pour vérifier la syntaxe
6. Cliquez sur **Enregistrer** pour sauvegarder la règle

### Test d'une règle YARA

1. Accédez à l'onglet **Règles YARA** dans le menu de navigation
2. Sélectionnez la règle à tester dans la liste
3. Cliquez sur **Tester la règle**
4. Sélectionnez un fichier ou un répertoire à analyser
5. Cliquez sur **Démarrer le test**
6. Consultez les résultats du test

## Génération de rapports

Le module Rapports permet de créer des rapports détaillés sur les analyses forensiques effectuées.

### Génération d'un rapport HTML

1. Accédez à l'onglet **Rapports** dans le menu de navigation
2. Cliquez sur **Nouveau rapport**
3. Sélectionnez le cas pour lequel générer un rapport
4. Configurez les options du rapport :
   - **Format** : Sélectionnez HTML
   - **Inclure détails des menaces** : Ajoute les détails des menaces détectées
   - **Inclure timeline** : Ajoute une chronologie des événements
   - **Inclure preuve blockchain** : Ajoute les preuves certifiées sur la blockchain
5. Cliquez sur **Générer le rapport**
6. Une fois le rapport généré, cliquez sur **Ouvrir** pour le visualiser
7. Pour sauvegarder le rapport, cliquez sur **Exporter**

### Gestion des rapports

1. Accédez à l'onglet **Rapports** dans le menu de navigation
2. Consultez la liste des rapports générés
3. Pour ouvrir un rapport, cliquez sur le bouton **Ouvrir**
4. Pour exporter un rapport, cliquez sur le bouton **Exporter**
5. Pour supprimer un rapport, cliquez sur le bouton **Supprimer**

## Intégration blockchain

Le module Blockchain permet de certifier et vérifier l'intégrité des preuves forensiques.

### Certification d'une preuve

1. Accédez à l'onglet **Blockchain** dans le menu de navigation
2. Cliquez sur **Nouvelle certification**
3. Sélectionnez la preuve à certifier
4. Vérifiez les informations de la preuve
5. Cliquez sur **Certifier**
6. Attendez la confirmation de la transaction blockchain
7. Consultez le hash de transaction généré

### Vérification d'une preuve

1. Accédez à l'onglet **Blockchain** dans le menu de navigation
2. Cliquez sur **Vérifier une preuve**
3. Sélectionnez la preuve à vérifier
4. Cliquez sur **Vérifier**
5. Consultez le résultat de la vérification

### Chaîne de custody

1. Accédez à l'onglet **Blockchain** dans le menu de navigation
2. Cliquez sur **Chaîne de custody**
3. Sélectionnez l'artefact pour lequel consulter la chaîne de custody
4. Consultez l'historique complet des actions effectuées sur l'artefact
5. Pour ajouter une entrée, cliquez sur **Ajouter une entrée**
6. Remplissez les informations requises et cliquez sur **Enregistrer**

## Fonctionnalités avancées

### Analyse de mémoire

1. Accédez à l'onglet **Analyse** dans le menu de navigation
2. Cliquez sur **Analyse avancée**
3. Sélectionnez **Analyse de mémoire**
4. Sélectionnez le dump mémoire à analyser
5. Configurez les options d'analyse :
   - **Extraction de processus** : Extrait les processus en mémoire
   - **Extraction de connexions** : Identifie les connexions réseau
   - **Extraction de clés** : Recherche des clés cryptographiques
6. Cliquez sur **Démarrer l'analyse**
7. Consultez les résultats dans l'onglet **Résultats**

### Analyse temporelle

1. Accédez à l'onglet **Analyse** dans le menu de navigation
2. Cliquez sur **Analyse avancée**
3. Sélectionnez **Analyse temporelle**
4. Sélectionnez les sources de données à inclure dans l'analyse
5. Définissez la période d'intérêt
6. Cliquez sur **Générer la timeline**
7. Explorez la timeline interactive générée

### Corrélation multi-source

1. Accédez à l'onglet **Analyse** dans le menu de navigation
2. Cliquez sur **Analyse avancée**
3. Sélectionnez **Corrélation multi-source**
4. Sélectionnez les différentes sources de données à corréler
5. Définissez les critères de corrélation
6. Cliquez sur **Démarrer la corrélation**
7. Consultez les résultats dans l'onglet **Résultats**

## Dépannage

### Problèmes courants et solutions

#### L'application ne démarre pas

- Vérifiez que Python est correctement installé
- Vérifiez que PyQt5 est correctement installé
- Consultez les logs dans `%APPDATA%\IRIS-Forensic-X\Logs`

#### Erreur lors de l'analyse VMDK

- Vérifiez que le fichier VMDK est accessible et non corrompu
- Vérifiez que vous disposez des permissions nécessaires
- Essayez de monter le VMDK manuellement pour vérifier son intégrité

#### Règles YARA non appliquées

- Vérifiez que les règles sont activées
- Vérifiez la syntaxe des règles
- Vérifiez que le moteur YARA est correctement installé

#### Erreur de génération de rapport

- Vérifiez que le répertoire de rapports est accessible en écriture
- Vérifiez que le cas contient des données à inclure dans le rapport
- Consultez les logs pour plus de détails sur l'erreur

### Logs d'application

Les logs de l'application sont stockés dans le répertoire `%APPDATA%\IRIS-Forensic-X\Logs`. Consultez ces fichiers pour obtenir des informations détaillées sur les erreurs rencontrées.

### Support technique

Si vous rencontrez des problèmes non résolus par ce manuel, veuillez contacter notre support technique :

- Site web : [www.iris-forensic-x.com/support](http://www.iris-forensic-x.com/support)
- Email : support@iris-forensic-x.com
- Téléphone : +33 (0)1 23 45 67 89
