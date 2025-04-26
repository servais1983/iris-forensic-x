# Guide d'installation d'IRIS-Forensic X

Ce guide détaille les étapes d'installation d'IRIS-Forensic X, un outil forensique hybride avancé pour l'analyse de cyberattaques.

## Prérequis système

- Système d'exploitation : Windows 10/11 (64 bits)
- Processeur : Intel Core i5 ou équivalent (4 cœurs recommandés)
- Mémoire : 8 Go RAM minimum (16 Go recommandés)
- Espace disque : 2 Go minimum pour l'application (+ espace pour les données d'analyse)
- Résolution d'écran : 1920x1080 ou supérieure
- Connexion Internet : Requise pour l'installation initiale et les mises à jour

## Installation automatique

### Méthode 1 : Installation via le package MSI (recommandée)

1. Téléchargez le fichier d'installation `IRIS-Forensic-X-Setup.msi` depuis le site officiel
2. Double-cliquez sur le fichier MSI pour lancer l'installation
3. Suivez les instructions de l'assistant d'installation
4. Une fois l'installation terminée, IRIS-Forensic X sera disponible dans le menu Démarrer

L'installateur MSI effectue automatiquement les opérations suivantes :
- Installation des fichiers de l'application
- Vérification et installation de Python si nécessaire
- Installation des dépendances Python requises (PyQt5, numpy, pandas, etc.)
- Création des raccourcis dans le menu Démarrer et sur le Bureau
- Configuration initiale de l'application

### Méthode 2 : Installation manuelle

Si vous rencontrez des problèmes avec l'installateur MSI, vous pouvez procéder à une installation manuelle :

1. Téléchargez l'archive ZIP d'IRIS-Forensic X depuis le site officiel
2. Extrayez l'archive dans un dossier de votre choix (par exemple, `C:\Program Files\IRIS-Forensic X`)
3. Ouvrez une invite de commande en tant qu'administrateur
4. Naviguez vers le dossier d'installation : `cd "C:\Program Files\IRIS-Forensic X"`
5. Exécutez le script d'installation : `install.bat`
6. Suivez les instructions à l'écran

## Résolution des problèmes courants

### Problème avec l'installation de PyQt5

Si vous rencontrez l'erreur `ModuleNotFoundError: No module named 'PyQt5'` lors du lancement de l'application :

1. Ouvrez une invite de commande en tant qu'administrateur
2. Exécutez la commande : `pip install PyQt5==5.15.6`
3. Si l'erreur persiste, essayez : `pip install PyQt5-sip`
4. Redémarrez l'application

### Python non détecté

Si l'installateur ne détecte pas Python :

1. Téléchargez et installez Python 3.10 ou supérieur depuis [python.org](https://www.python.org/downloads/)
2. Assurez-vous de cocher l'option "Add Python to PATH" lors de l'installation
3. Relancez l'installation d'IRIS-Forensic X

### Erreur de privilèges insuffisants

Si vous obtenez une erreur concernant des privilèges insuffisants :

1. Fermez l'installateur
2. Cliquez-droit sur le fichier d'installation et sélectionnez "Exécuter en tant qu'administrateur"
3. Suivez les instructions de l'assistant d'installation

## Installation silencieuse (pour les administrateurs système)

Pour déployer IRIS-Forensic X sur plusieurs postes, vous pouvez utiliser l'installation silencieuse :

```
msiexec /i IRIS-Forensic-X-Setup.msi /qn
```

Options supplémentaires :
- `/qb` : Mode silencieux avec interface de base
- `INSTALLDIR="C:\Chemin\Personnalisé"` : Spécifier un répertoire d'installation personnalisé
- `DESKTOP_SHORTCUT=0` : Ne pas créer de raccourci sur le Bureau

## Désinstallation

### Désinstallation via le Panneau de configuration

1. Ouvrez le Panneau de configuration
2. Accédez à "Programmes et fonctionnalités" ou "Applications et fonctionnalités"
3. Sélectionnez "IRIS-Forensic X" dans la liste
4. Cliquez sur "Désinstaller" et suivez les instructions

### Désinstallation manuelle

1. Naviguez vers le dossier d'installation d'IRIS-Forensic X
2. Exécutez le script `uninstall.bat` en tant qu'administrateur
3. Suivez les instructions à l'écran

## Vérification de l'installation

Pour vérifier que l'installation s'est correctement déroulée :

1. Lancez IRIS-Forensic X depuis le menu Démarrer ou le raccourci sur le Bureau
2. L'application devrait démarrer sans erreur et afficher l'écran d'accueil
3. Accédez à l'onglet "À propos" pour vérifier la version installée

## Mise à jour

IRIS-Forensic X vérifie automatiquement les mises à jour au démarrage. Si une mise à jour est disponible, vous serez invité à l'installer.

Pour vérifier manuellement les mises à jour :
1. Ouvrez IRIS-Forensic X
2. Accédez au menu "Aide" > "Vérifier les mises à jour"
3. Suivez les instructions à l'écran si une mise à jour est disponible

## Support technique

Si vous rencontrez des problèmes lors de l'installation, veuillez consulter notre base de connaissances ou contacter notre support technique :

- Site web : [www.iris-forensic-x.com/support](http://www.iris-forensic-x.com/support)
- Email : support@iris-forensic-x.com
- Téléphone : +33 (0)1 23 45 67 89
