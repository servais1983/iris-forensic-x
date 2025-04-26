# IRIS-Forensic X

IRIS-Forensic X est un outil forensique hybride avancé pour l'analyse de cyberattaques, spécialement conçu pour les analystes en cybersécurité.

## Présentation

IRIS-Forensic X combine une interface CLI puissante, une interface graphique intuitive, des capacités d'IA avancées et une intégration blockchain pour l'analyse forensique complète. Il permet d'analyser efficacement les fichiers VMDK, logs et CSV pour détecter des malwares, ransomwares, traces de phishing, backdoors et autres indicateurs de compromission.

### Principales fonctionnalités

- **Collecte intelligente "3-en-1"** : Capture de mémoire, logs et trafic réseau
- **Analyse unifiée IA/Classique** : Détection automatique des menaces avec validation humaine
- **Support multi-format** : Analyse de fichiers VMDK, logs, CSV et autres formats
- **Intégration de règles YARA** : Détection personnalisable de menaces
- **Système de scoring** : Évaluation automatique du niveau de menace
- **Génération de rapports HTML** : Documentation détaillée des résultats d'analyse
- **Certification blockchain** : Garantie d'intégrité des preuves forensiques

## Prérequis système

- Système d'exploitation : Windows 10/11 (64 bits)
- Processeur : Intel Core i5 ou équivalent (4 cœurs recommandés)
- Mémoire : 8 Go RAM minimum (16 Go recommandés)
- Espace disque : 2 Go minimum pour l'application (+ espace pour les données d'analyse)
- Résolution d'écran : 1920x1080 ou supérieure
- Connexion Internet : Requise pour l'installation initiale et les mises à jour

## Installation depuis GitHub

### Méthode 1 : Clonage du dépôt et installation manuelle

1. **Cloner le dépôt GitHub**
   ```
   git clone https://github.com/servais1983/iris-forensic-x.git
   cd iris-forensic-x
   ```

2. **Installer les dépendances requises**
   ```
   # Installer Python et les dépendances Python
   python -m pip install -r requirements.txt
   
   # Installer les dépendances .NET (si non installées)
   # Télécharger et installer .NET SDK 6.0 ou supérieur depuis https://dotnet.microsoft.com/download
   ```

3. **Compiler l'application Windows**
   ```
   cd windows
   # Utiliser Visual Studio ou MSBuild pour compiler la solution
   # Exemple avec MSBuild :
   msbuild IRIS.sln /p:Configuration=Release
   ```

4. **Exécuter l'installateur**
   ```
   cd installer
   install.bat
   ```

### Méthode 2 : Téléchargement et installation du package MSI

1. **Télécharger le package d'installation**
   - Accéder à la section "Releases" du dépôt GitHub
   - Télécharger le fichier MSI le plus récent

2. **Exécuter l'installateur MSI**
   - Double-cliquer sur le fichier MSI téléchargé
   - Suivre les instructions à l'écran pour terminer l'installation

### Méthode 3 : Script d'installation automatisé

1. **Télécharger le script d'installation**
   ```
   curl -o install_iris.bat https://raw.githubusercontent.com/servais1983/iris-forensic-x/main/windows/installer/install.bat
   ```

2. **Exécuter le script d'installation**
   ```
   install_iris.bat
   ```
   Ce script téléchargera automatiquement tous les composants nécessaires et installera l'application.

## Documentation

- [Manuel d'utilisation](docs/manuel_utilisation.md) - Guide complet des fonctionnalités
- [Guide d'installation](docs/guide_installation.md) - Instructions d'installation détaillées
- [Guide de test et déploiement](docs/guide_test_deploiement.md) - Procédures de test et déploiement en entreprise

## Architecture

IRIS-Forensic X est construit selon une architecture modulaire qui comprend :

- **Core CLI** : Moteur d'analyse forensique en ligne de commande
- **Interface Windows** : Application graphique basée sur WPF
- **Modules d'analyse** : Composants spécialisés pour différents types d'analyse
- **Intégration IA** : Modèles de détection de menaces basés sur l'apprentissage automatique
- **Intégration blockchain** : Certification des preuves forensiques

## Développement

### Structure du projet

- **core-cli/** - Moteur d'analyse en ligne de commande
- **windows/** - Application Windows (WPF)
  - **IRIS.Models/** - Modèles de données
  - **IRIS.Services/** - Services d'application
  - **IRIS.ViewModels/** - Modèles de vue (MVVM)
  - **IRIS.Views/** - Vues de l'interface utilisateur
  - **IRIS.Windows/** - Classes principales de l'application
- **ai/** - Modèles et services d'intelligence artificielle
- **blockchain/** - Intégration et certification blockchain
- **docs/** - Documentation complète

## Résolution des problèmes courants

### Erreur "ModuleNotFoundError: No module named 'PyQt5'"
Si vous rencontrez cette erreur lors de l'installation, exécutez la commande suivante :
```
python -m pip install PyQt5
```

### Problèmes d'accès aux fichiers VMDK
Assurez-vous que l'application est exécutée avec des privilèges administratifs pour accéder aux fichiers VMDK.

### Erreurs de compilation de l'application Windows
Vérifiez que vous avez installé .NET SDK 6.0 ou supérieur et que tous les packages NuGet sont correctement restaurés.

## Licence

IRIS-Forensic X est distribué sous licence propriétaire. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

## Support

Pour toute question ou assistance technique, veuillez contacter :

- Site web : [www.iris-forensic-x.com/support](http://www.iris-forensic-x.com/support)
- Email : support@iris-forensic-x.com
- Téléphone : +33 (0)1 23 45 67 89
