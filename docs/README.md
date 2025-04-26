# IRIS-Forensic X - Documentation

## Vue d'ensemble

IRIS-Forensic X est une solution forensique hybride de pointe combinant une interface en ligne de commande (CLI) puissante, une interface web intuitive, des capacités d'intelligence artificielle avancées et une intégration blockchain pour la preuve légale. Conçue pour les analystes en cybersécurité, cette plateforme permet d'effectuer des analyses forensiques complètes sur divers types de données.

## Architecture

IRIS-Forensic X repose sur une architecture hybride composée de quatre couches principales :

1. **Core CLI** (Go/Rust) - Pour les opérations rapides et l'accès système de bas niveau
2. **WebUI** (Next.js/React) - Pour l'analyse visuelle et l'interaction utilisateur
3. **Couche IA** (Python/ONNX) - Pour la détection automatisée et l'analyse avancée
4. **Preuve légale** (Blockchain) - Pour la certification et la traçabilité des preuves

## Fonctionnalités principales

### 1. Collecte intelligente "3-en-1"

La fonctionnalité `capture` permet de collecter des données de trois manières différentes :

- **Snapshot mémoire eBPF** - Capture l'état de la mémoire d'un système
- **Hyper-Ingestor logs** - Collecte et normalise les fichiers journaux
- **Capture réseau contextuelle** - Enregistre le trafic réseau pertinent

Exemple d'utilisation :
```bash
$ iris capture --smart
```

### 2. Analyse unifiée AI/Classique

La fonctionnalité `analyze` combine des techniques d'analyse traditionnelles et d'intelligence artificielle :

- **Analyse Autopsy-like** - Utilise The Sleuth Kit (TSK++) pour l'analyse de disque
- **Détection IA CodeBERT** - Identifie le code malveillant et les comportements suspects
- **Reconstruction temporelle** - Établit une chronologie des événements

Exemple d'utilisation :
```bash
$ iris analyze --all
```

### 3. Triage et priorisation

La fonctionnalité `triage` permet de prioriser les artefacts et les alertes :

- **Scoring MITRE ATT&CK** - Évalue les menaces selon le framework MITRE
- **Clustering d'alertes** - Regroupe les alertes similaires
- **Priorisation contextuelle** - Adapte la priorité selon le contexte

Exemple d'utilisation :
```bash
$ iris triage --context enterprise
```

### 4. Réponse et remédiation

La fonctionnalité `respond` propose des actions de remédiation :

- **Playbooks automatisés** - Exécute des actions prédéfinies
- **Isolation d'artefacts** - Isole les fichiers ou processus malveillants
- **Génération de règles** - Crée des règles de détection pour les systèmes de sécurité

Exemple d'utilisation :
```bash
$ iris respond --auto
```

### 5. Certification blockchain

La fonctionnalité `certify` garantit l'intégrité des preuves :

- **Chaîne de custody immuable** - Enregistre toutes les manipulations des preuves
- **Horodatage cryptographique** - Certifie le moment de la collecte
- **Vérification d'intégrité** - Permet de vérifier que les preuves n'ont pas été altérées

Exemple d'utilisation :
```bash
$ iris certify --evidence file.bin
```

## Interface Web

L'interface web d'IRIS-Forensic X offre une expérience utilisateur intuitive avec les sections suivantes :

### Dashboard

Vue d'ensemble des analyses en cours, des menaces détectées et des statistiques.

### Capture

Interface pour lancer et gérer les captures de données.

### Analyze

Outils d'analyse visuelle et interactive des données collectées.

### Triage

Système de priorisation et de gestion des alertes.

### Respond

Outils de réponse aux incidents et de remédiation.

### AI

Interface pour les modèles d'IA et l'apprentissage automatique.

### Blockchain

Gestion des preuves légales et de la chaîne de custody.

## Installation

### Prérequis

- Système d'exploitation : Linux (Ubuntu 20.04+), Windows 10/11, macOS 12+
- RAM : 8 Go minimum, 16 Go recommandé
- Espace disque : 10 Go minimum
- Dépendances : Docker, Python 3.9+, Node.js 18+

### Installation native

```bash
# Option 1 : Installation automatique
curl -sSL https://get.iris-forensic.com | bash

# Option 2 : Installation manuelle
git clone https://github.com/iris-forensic/iris-x.git
cd iris-x
./install.sh
```

### Installation Docker

```bash
# Démarrage rapide avec Docker
docker run -it irisx/quickstart --demo ai-ransomware

# Installation complète avec Docker Compose
git clone https://github.com/iris-forensic/iris-x.git
cd iris-x
docker-compose up -d
```

## Configuration

La configuration d'IRIS-Forensic X se fait via le fichier `config.yaml` :

```yaml
# Exemple de configuration
iris:
  core:
    temp_dir: /tmp/iris-x
    log_level: info
  
  capture:
    max_size: 10GB
    formats:
      - raw
      - ewf
      - aff4
  
  ai:
    models_dir: /opt/iris-x/models
    enable_gpu: true
  
  blockchain:
    provider: hyperledger
    storage: local
```

## Intégration avec d'autres outils

IRIS-Forensic X s'intègre avec de nombreux outils forensiques et de sécurité :

- **Volatility** - Pour l'analyse de mémoire
- **Autopsy/TSK** - Pour l'analyse de disque
- **Wireshark/Zeek** - Pour l'analyse réseau
- **MISP** - Pour le partage de renseignements sur les menaces
- **TheHive/Cortex** - Pour la gestion des incidents
- **Elastic Stack** - Pour l'analyse de logs

## Cas d'utilisation

### Analyse de malware

1. Capturez un snapshot mémoire : `iris capture --memory`
2. Analysez le malware : `iris analyze --malware`
3. Générez un rapport : `iris report --format pdf`

### Investigation d'incident

1. Collectez les logs : `iris capture --logs`
2. Reconstruisez la chronologie : `iris timeline --auto`
3. Identifiez les IOCs : `iris extract --iocs`
4. Certifiez les preuves : `iris certify --all`

### Analyse forensique de disque

1. Créez une image disque : `iris capture --disk /dev/sda`
2. Analysez le système de fichiers : `iris analyze --filesystem`
3. Récupérez les fichiers supprimés : `iris recover --deleted`
4. Générez un rapport : `iris report --comprehensive`

## Bonnes pratiques

- Toujours travailler sur des copies des données, jamais sur les originaux
- Utiliser la certification blockchain pour toutes les preuves importantes
- Documenter chaque étape de l'analyse
- Mettre à jour régulièrement les modèles d'IA
- Sauvegarder les configurations et les rapports

## Dépannage

### Problèmes courants

- **Erreur de capture** : Vérifiez les permissions et l'espace disque
- **Modèles IA non chargés** : Vérifiez le dossier des modèles et la connexion internet
- **Erreur blockchain** : Vérifiez la configuration du fournisseur blockchain
- **Interface web inaccessible** : Vérifiez le service nginx et les ports

### Journaux de diagnostic

Les journaux se trouvent dans les emplacements suivants :
- CLI : `/var/log/iris-x/core.log`
- WebUI : `/var/log/iris-x/webui.log`
- IA : `/var/log/iris-x/ai.log`
- Blockchain : `/var/log/iris-x/blockchain.log`

## Support et communauté

- **Documentation officielle** : [docs.iris-forensic.com](https://docs.iris-forensic.com)
- **Forum communautaire** : [community.iris-forensic.com](https://community.iris-forensic.com)
- **GitHub** : [github.com/iris-forensic/iris-x](https://github.com/iris-forensic/iris-x)
- **Discord** : [discord.gg/iris-forensic](https://discord.gg/iris-forensic)

## Feuille de route

- **Q1 2025** : Intégration M365/AWS Logs
- **Q2 2025** : Assistant Vocale (type Copilot)
- **Q3 2025** : Module Quantum-Safe (CRYSTALS-Kyber)
- **Q4 2025** : Autopsy Replacement Toolkit

## Licence

IRIS-Forensic X est distribué sous licence open-source Apache 2.0.

---

© 2025 IRIS-Forensic Team. Tous droits réservés.
