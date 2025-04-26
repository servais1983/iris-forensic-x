# Guide d'installation et de déploiement - IRIS-Forensic X pour Windows

Ce guide détaille les étapes pour installer et déployer IRIS-Forensic X sur des systèmes Windows en environnement d'entreprise.

## Prérequis système

- **Système d'exploitation** : Windows 10/11 64-bit (Pro, Enterprise ou Education)
- **Processeur** : Intel Core i5/AMD Ryzen 5 ou supérieur (4 cœurs minimum)
- **Mémoire RAM** : 16 Go minimum, 32 Go recommandé
- **Espace disque** : 20 Go minimum pour l'installation, plus espace pour les données d'analyse
- **Résolution d'écran** : 1920x1080 minimum
- **Droits d'administration** : Nécessaires pour l'installation

## Installation standard

### 1. Téléchargement

1. Téléchargez le dernier installateur depuis [https://iris-forensic.com/download](https://iris-forensic.com/download)
2. Vérifiez l'intégrité du fichier avec la somme de contrôle SHA-256 fournie

### 2. Installation

1. Exécutez `IRIS-Forensic-X-Setup.exe` avec des droits d'administrateur
2. Suivez les instructions de l'assistant d'installation
3. Sélectionnez les composants à installer :
   - Application principale (obligatoire)
   - Modèles d'IA (recommandé)
   - Support blockchain (optionnel)
   - Outils CLI (recommandé)
   - Documentation hors ligne (optionnel)
4. Choisissez le dossier d'installation (par défaut : `C:\Program Files\IRIS-Forensic-X\`)
5. Attendez la fin de l'installation

### 3. Configuration initiale

Au premier démarrage, l'assistant de configuration vous guidera pour :
- Créer un profil utilisateur
- Configurer les dossiers de travail
- Définir les paramètres de performance
- Configurer les intégrations (optionnel)

## Installation silencieuse (déploiement d'entreprise)

Pour un déploiement automatisé sur plusieurs postes :

```cmd
IRIS-Forensic-X-Setup.exe /S /D=C:\IRIS-X /COMPONENTS=main,ai,cli /CONFIG=config.json
```

Paramètres disponibles :
- `/S` : Installation silencieuse
- `/D=chemin` : Dossier d'installation
- `/COMPONENTS=comp1,comp2` : Composants à installer
- `/CONFIG=fichier.json` : Fichier de configuration pré-défini
- `/NORESTART` : Ne pas redémarrer après l'installation

## Fichier de configuration (config.json)

```json
{
  "user": {
    "name": "Forensic Analyst",
    "organization": "Enterprise Security Team"
  },
  "workspace": {
    "default_path": "D:\\IRIS-X-Workspace",
    "temp_path": "D:\\IRIS-X-Temp"
  },
  "performance": {
    "max_memory": "16GB",
    "max_threads": 8,
    "gpu_acceleration": true
  },
  "integrations": {
    "active_directory": {
      "enabled": true,
      "server": "ad.company.com",
      "domain": "COMPANY"
    },
    "siem": {
      "enabled": false
    }
  }
}
```

## Installation portable

Pour une version portable (sans installation) :

1. Téléchargez `IRIS-Forensic-X-Portable.zip`
2. Extrayez l'archive vers un dossier de votre choix (ex: clé USB, disque externe)
3. Exécutez `IRIS-X-Portable.exe`

Limitations de la version portable :
- Certaines fonctionnalités avancées peuvent être limitées
- Les performances peuvent être réduites
- Nécessite toujours des droits d'administration pour certaines opérations

## Mise à jour

### Mise à jour automatique

Par défaut, l'application vérifie les mises à jour au démarrage. Lorsqu'une mise à jour est disponible :

1. Une notification apparaît
2. Cliquez sur "Télécharger et installer"
3. L'application redémarre automatiquement après la mise à jour

### Mise à jour manuelle

1. Téléchargez la dernière version depuis le site officiel
2. Fermez l'application si elle est en cours d'exécution
3. Exécutez l'installateur, qui détectera l'installation existante
4. Suivez les instructions pour mettre à jour

### Mise à jour en entreprise

Pour les environnements d'entreprise avec restrictions :

1. Téléchargez le package de mise à jour (`IRIS-X-Update.msp`)
2. Déployez via GPO ou votre outil de gestion de parc
3. Utilisez la commande : `msiexec /p IRIS-X-Update.msp /qn`

## Configuration système avancée

### Optimisation des performances

Modifiez le fichier `C:\Program Files\IRIS-Forensic-X\config\performance.yaml` :

```yaml
memory:
  max_usage_percent: 75
  swap_path: "D:\\IRIS-X-Swap"

processing:
  threads: 8
  batch_size: 1000
  priority: normal  # options: low, normal, high

gpu:
  enabled: true
  memory_limit: 4GB
  precision: mixed  # options: full, mixed, half
```

### Configuration réseau

Modifiez le fichier `C:\Program Files\IRIS-Forensic-X\config\network.yaml` :

```yaml
proxy:
  enabled: false
  server: "proxy.company.com"
  port: 8080
  auth:
    username: ""
    password_encrypted: ""

api:
  local_port: 8765
  bind_address: "127.0.0.1"
  
ssl:
  verify: true
  cert_path: ""
```

## Intégration avec l'Active Directory

Pour l'authentification via Active Directory :

1. Exécutez `IRIS-X-ADConfig.exe` avec des droits d'administrateur
2. Configurez les paramètres de connexion à l'AD
3. Définissez les groupes autorisés et leurs niveaux d'accès
4. Testez la connexion
5. Appliquez les paramètres

## Dépannage

### Problèmes d'installation

| Problème | Solution |
|----------|----------|
| Erreur 1603 | Vérifiez les droits d'administrateur et désactivez temporairement l'antivirus |
| Erreur de dépendance | Installez les prérequis manquants (.NET Framework, Visual C++ Redistributable) |
| Conflit de port | Modifiez le port dans `network.yaml` après l'installation |

### Problèmes de démarrage

| Problème | Solution |
|----------|----------|
| Crash au démarrage | Vérifiez les logs dans `%APPDATA%\IRIS-Forensic-X\logs` |
| Erreur de base de données | Exécutez `IRIS-X-Repair.exe --db-repair` |
| Interface bloquée | Supprimez le fichier `%APPDATA%\IRIS-Forensic-X\cache\ui-state.json` |

### Récupération

En cas de corruption :

1. Sauvegardez vos données depuis `%USERPROFILE%\Documents\IRIS-Forensic-X\`
2. Désinstallez l'application
3. Supprimez les dossiers résiduels dans `%APPDATA%` et `%PROGRAMDATA%`
4. Réinstallez l'application
5. Restaurez vos données

## Désinstallation

### Désinstallation standard

1. Ouvrez le Panneau de configuration > Programmes et fonctionnalités
2. Sélectionnez "IRIS-Forensic X"
3. Cliquez sur "Désinstaller"
4. Suivez les instructions

### Désinstallation silencieuse

```cmd
IRIS-Forensic-X-Setup.exe /S /UNINSTALL
```

ou

```cmd
msiexec /x {PRODUCT-GUID} /qn
```

## Support technique

- **Site web** : [support.iris-forensic.com](https://support.iris-forensic.com)
- **Email** : support@iris-forensic.com
- **Téléphone** : +1-555-FORENSIC (disponible 24/7)

---

© 2025 IRIS-Forensic Team. Tous droits réservés.
