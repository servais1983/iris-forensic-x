## Guide d'utilisation - IRIS-Forensic X

Ce guide détaille les procédures opérationnelles pour utiliser efficacement IRIS-Forensic X dans un contexte d'investigation forensique professionnelle.

### Procédures d'investigation

#### 1. Préparation de l'environnement

Avant de commencer une investigation :

```bash
# Vérifier l'intégrité de l'environnement
$ iris check-environment

# Préparer un espace de travail isolé
$ iris workspace create --name "investigation-2025-04-25"
```

#### 2. Acquisition des données

Plusieurs méthodes d'acquisition sont disponibles selon le contexte :

```bash
# Acquisition d'une image mémoire
$ iris capture memory --target 192.168.1.10 --output memory.dump

# Acquisition d'une image disque
$ iris capture disk --device /dev/sda --format E01

# Acquisition de logs
$ iris capture logs --source "windows-event" --timeframe "last-24h"
```

#### 3. Analyse préliminaire

Effectuez une analyse rapide pour identifier les zones d'intérêt :

```bash
# Analyse automatique avec IA
$ iris analyze --auto --input memory.dump

# Extraction des indicateurs de compromission
$ iris extract iocs --input memory.dump
```

#### 4. Analyse approfondie

Concentrez-vous sur les éléments suspects identifiés :

```bash
# Analyse de processus suspects
$ iris analyze process --pid 1234 --memory memory.dump

# Analyse de fichiers malveillants
$ iris analyze malware --file suspicious.exe --sandbox

# Analyse de connexions réseau
$ iris analyze network --pcap capture.pcap --detect-c2
```

#### 5. Chronologie des événements

Reconstruisez la séquence des événements :

```bash
# Création d'une chronologie automatique
$ iris timeline --auto --sources "all"

# Ajout manuel d'événements
$ iris timeline add --timestamp "2025-04-25T14:30:00" --description "Exécution de malware"
```

#### 6. Documentation et certification

Documentez et certifiez vos découvertes :

```bash
# Certification d'une preuve
$ iris certify --evidence malware.bin --description "Malware extrait de la mémoire"

# Génération d'un rapport
$ iris report generate --template "forensic-full" --output report.pdf
```

### Cas d'utilisation spécifiques

#### Investigation de ransomware

```bash
# Détection des indicateurs de ransomware
$ iris detect ransomware --input system.dump

# Récupération de clés potentielles
$ iris recover keys --ransomware-type "lockbit3" --memory memory.dump

# Analyse des modifications de registre
$ iris analyze registry --focus "ransomware" --input registry.dat
```

#### Analyse d'intrusion APT

```bash
# Détection de persistance
$ iris detect persistence --input system.image

# Analyse de mouvement latéral
$ iris analyze lateral-movement --network network.pcap --logs auth.log

# Corrélation avec des TTPs connus
$ iris correlate --mitre-attack --input all-artifacts
```

#### Analyse de phishing

```bash
# Extraction et analyse d'emails
$ iris extract emails --input outlook.pst

# Analyse d'URLs et de pièces jointes
$ iris analyze urls --input extracted-urls.txt
$ iris analyze attachments --input attachments-folder/

# Reconstruction de la campagne
$ iris reconstruct campaign --artifacts "all" --ai-assist
```

### Bonnes pratiques pour la chaîne de custody

1. **Documenter chaque étape**
   ```bash
   $ iris custody log --action "Acquisition mémoire" --actor "John Doe" --notes "Acquisition sans erreur"
   ```

2. **Vérifier régulièrement l'intégrité**
   ```bash
   $ iris verify --all-evidence
   ```

3. **Exporter la chaîne complète**
   ```bash
   $ iris custody export --format "legal" --output custody-chain.pdf
   ```

### Intégration avec l'écosystème de sécurité

```bash
# Export vers MISP
$ iris export misp --iocs extracted-iocs.json --server misp.company.com

# Création de règles YARA
$ iris generate yara --from-malware malware.bin --output detection.yar

# Création de règles Sigma
$ iris generate sigma --from-logs suspicious-logs/ --output detection.yml
```

### Dépannage avancé

```bash
# Diagnostic complet
$ iris diagnose --all

# Réparation de la base de données
$ iris repair database

# Récupération de session
$ iris recover session --id "session-2025-04-25"
```

---

Ce guide est conçu pour les analystes forensiques professionnels. Pour plus d'informations, consultez la documentation complète ou contactez le support technique.
