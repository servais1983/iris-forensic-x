# Architecture adaptée pour Windows - IRIS-Forensic X

Ce document détaille l'architecture adaptée d'IRIS-Forensic X pour une application Windows native, transportable et déployable en entreprise.

## Vue d'ensemble

IRIS-Forensic X pour Windows est conçu comme une application native autonome, tout en conservant l'architecture hybride qui fait sa force. L'application principale est développée en C# avec .NET pour l'interface utilisateur Windows, tandis que les composants spécialisés (CLI, IA, blockchain) sont encapsulés et intégrés de manière transparente.

## Architecture système

```
┌─────────────────────────────────────────────────────────────┐
│                  IRIS-Forensic X pour Windows               │
├─────────────┬─────────────┬─────────────┬─────────────┬─────┤
│  Interface  │    Core     │  Moteur IA  │ Blockchain  │ API │
│  Windows    │    Engine   │             │             │     │
│  (C#/.NET)  │  (C++/Go)   │  (Python)   │  (Go/C#)    │     │
├─────────────┴─────────────┴─────────────┴─────────────┴─────┤
│                    Couche d'intégration                     │
├─────────────┬─────────────┬─────────────┬─────────────┬─────┤
│  Système    │  Gestion    │  Stockage   │  Sécurité   │ Log │
│  de fichiers│  mémoire    │  de données │             │     │
└─────────────┴─────────────┴─────────────┴─────────────┴─────┘
```

## Composants principaux

### 1. Interface Windows (C#/.NET)

- **Framework** : WPF (Windows Presentation Foundation) pour une interface riche et moderne
- **Design** : Inspiré de Autopsy/Sleuth Kit avec une UX améliorée
- **Modèle** : MVVM (Model-View-ViewModel) pour une séparation claire des responsabilités
- **Contrôles** : Personnalisés pour l'analyse forensique (visualiseurs hexadécimaux, chronologies, graphes)

### 2. Core Engine (C++/Go)

- **Noyau d'analyse** : Écrit en C++ pour des performances optimales
- **Modules CLI** : Encapsulés depuis la version Go originale
- **Interopérabilité** : Via P/Invoke et bibliothèques partagées (.dll)
- **Multi-threading** : Exploitation optimale des ressources système

### 3. Moteur IA (Python)

- **Runtime Python** : Embarqué et isolé (pas de dépendance externe)
- **Modèles** : Précompilés et optimisés pour ONNX Runtime
- **Inférence** : Accélération GPU via CUDA/DirectML
- **Interopérabilité** : Python.NET pour l'intégration transparente

### 4. Blockchain (Go/C#)

- **Implémentation hybride** : Core en Go, wrapper en C#
- **Stockage local** : Pour fonctionnement hors ligne
- **Synchronisation** : Optionnelle avec blockchain externe
- **Cryptographie** : Bibliothèques natives Windows pour les opérations cryptographiques

### 5. API et extensibilité

- **API interne** : Pour la communication entre composants
- **API externe** : Pour l'intégration avec d'autres outils
- **Système de plugins** : Architecture extensible via modules .NET
- **Scripting** : Support pour PowerShell et Python

## Gestion des données

### Stockage

- **Base de données** : SQLite embarquée pour la portabilité
- **Système de fichiers** : Accès optimisé via API Windows
- **Virtualisation** : Support pour VHD/VHDX et formats forensiques (E01, AFF, etc.)
- **Compression** : Algorithmes optimisés pour données forensiques

### Persistance

- **Configuration** : Fichiers XML/JSON dans le dossier d'application ou portable
- **Données utilisateur** : Séparées pour faciliter la sauvegarde/restauration
- **Cache** : Gestion intelligente pour optimiser les performances

## Déploiement et portabilité

### Installation standard

- **Installateur** : MSI/EXE avec options de personnalisation
- **Prérequis** : Détection et installation automatique des dépendances
- **Mise à jour** : Mécanisme intégré avec différentiel

### Version portable

- **Autonomie** : Tous les composants embarqués dans un dossier unique
- **Isolation** : Pas d'écriture en dehors du dossier d'application
- **Compatibilité** : Fonctionne depuis clé USB ou partage réseau

### Déploiement entreprise

- **GPO** : Support pour déploiement via stratégies de groupe
- **SCCM/Intune** : Packages de déploiement compatibles
- **Télémétrie** : Optionnelle et configurable pour environnements d'entreprise

## Sécurité

### Protection des données

- **Chiffrement** : AES-256 pour les données sensibles
- **Contrôle d'accès** : Intégration avec Active Directory
- **Audit** : Journalisation complète des actions

### Intégrité

- **Vérification** : Signatures numériques pour tous les composants
- **Isolation** : Sandboxing pour l'exécution de code suspect
- **Conformité** : Respect des normes de sécurité (NIST, etc.)

## Performances

### Optimisation

- **Multi-threading** : Exploitation optimale des processeurs multi-cœurs
- **Mémoire** : Gestion avancée avec pagination intelligente
- **GPU** : Accélération pour traitement IA et visualisation

### Évolutivité

- **Ressources** : Adaptation automatique selon la configuration
- **Distribution** : Possibilité de répartir les charges sur plusieurs machines
- **Limites** : Configuration des seuils pour éviter la saturation

## Intégration écosystème

### Outils forensiques

- **Autopsy/TSK** : Import/export compatible
- **EnCase** : Support des formats E01/Ex01
- **FTK** : Compatibilité des rapports

### Sécurité d'entreprise

- **SIEM** : Connecteurs pour Splunk, QRadar, etc.
- **EDR** : Intégration avec CrowdStrike, SentinelOne, etc.
- **SOAR** : API pour automatisation des workflows

## Considérations techniques

### Compatibilité Windows

- **Versions** : Windows 10/11 (64-bit)
- **Serveur** : Windows Server 2019/2022
- **UAC** : Gestion intelligente des privilèges
- **Antivirus** : Exclusions recommandées pour performances optimales

### Dépendances minimisées

- **.NET** : Framework 4.8 ou .NET 6+ (embarqué)
- **Visual C++** : Runtime embarqué
- **Drivers** : Installation automatique si nécessaire

### Empreinte système

- **Disque** : ~500 MB pour l'application, espace variable pour les données
- **RAM** : 4 GB minimum, utilisation adaptative
- **CPU** : Optimisé pour Intel/AMD modernes

## Feuille de route technique

### Phase 1 : Fondation Windows

- Portage de l'interface utilisateur en WPF
- Adaptation du Core Engine pour Windows
- Système de déploiement de base

### Phase 2 : Intégration avancée

- Intégration complète des composants IA et blockchain
- Optimisation des performances Windows
- Système de plugins

### Phase 3 : Entreprise et portabilité

- Fonctionnalités d'entreprise (AD, GPO, etc.)
- Version portable complète
- Intégration écosystème Windows

---

Cette architecture garantit qu'IRIS-Forensic X pour Windows conserve toutes les capacités avancées de la version originale, tout en offrant une expérience native Windows, une portabilité maximale et une facilité de déploiement en entreprise.
