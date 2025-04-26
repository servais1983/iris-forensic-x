# Guide de compilation de l'installateur IRIS-Forensic X

Ce document explique comment compiler l'installateur Windows pour IRIS-Forensic X en utilisant WiX Toolset.

## Prérequis

1. WiX Toolset v3.11 ou supérieur
2. Visual Studio 2019 ou supérieur avec les composants de développement C# et .NET
3. Windows 10 SDK

## Installation des prérequis

### WiX Toolset

1. Téléchargez WiX Toolset depuis https://wixtoolset.org/releases/
2. Installez WiX Toolset
3. Installez l'extension WiX Toolset pour Visual Studio

### Visual Studio

Assurez-vous que les composants suivants sont installés :
- Développement .NET Desktop
- Développement Windows Desktop
- Outils de build C++

### Windows 10 SDK

Installez le Windows 10 SDK depuis le Visual Studio Installer ou téléchargez-le séparément.

## Structure des fichiers

```
installer/
├── Product.wxs        # Définition principale de l'installateur
├── License.rtf        # Fichier de licence
├── AppIcon.ico        # Icône de l'application
├── install.bat        # Script d'installation alternatif
└── uninstall.bat      # Script de désinstallation
```

## Compilation de l'installateur

### Méthode 1 : Ligne de commande

1. Ouvrez une invite de commande en tant qu'administrateur
2. Naviguez vers le répertoire `installer`
3. Exécutez les commandes suivantes :

```batch
candle Product.wxs
light -ext WixUIExtension Product.wixobj -out IRIS-Forensic-X.msi
```

### Méthode 2 : Visual Studio

1. Ouvrez Visual Studio
2. Créez un nouveau projet "WiX Setup Project"
3. Ajoutez les fichiers existants au projet
4. Compilez le projet

## Personnalisation de l'installateur

### Modification des informations du produit

Modifiez les variables définies au début du fichier `Product.wxs` :

```xml
<?define ProductName="IRIS-Forensic X" ?>
<?define ProductVersion="1.0.0" ?>
<?define ProductManufacturer="IRIS Forensic Team" ?>
<?define ProductUpgradeCode="4A2F5EB2-C3F1-4E3A-B8D1-C5E7F5C8A9B0" ?>
```

### Ajout de fichiers supplémentaires

Pour ajouter des fichiers supplémentaires à l'installateur, modifiez la section `ProductComponents` dans `Product.wxs`.

### Modification de l'interface utilisateur

L'installateur utilise l'interface standard WixUI_InstallDir. Pour utiliser une autre interface, modifiez la ligne :

```xml
<UIRef Id="WixUI_InstallDir" />
```

## Distribution de l'installateur

Une fois compilé, l'installateur `IRIS-Forensic-X.msi` peut être distribué aux utilisateurs finaux. Pour une installation silencieuse, utilisez la commande :

```batch
msiexec /i IRIS-Forensic-X.msi /qn
```

## Utilisation des scripts alternatifs

En plus de l'installateur MSI, deux scripts batch sont fournis :

- `install.bat` : Script d'installation manuel
- `uninstall.bat` : Script de désinstallation manuel

Ces scripts peuvent être utilisés dans les environnements où l'utilisation de MSI n'est pas possible ou pour des déploiements personnalisés.

## Dépannage

### Erreurs de compilation

- **Erreur "ICE38"** : Assurez-vous que tous les composants ont un GUID unique
- **Erreur "ICE64"** : Vérifiez que tous les chemins de fichiers sont corrects
- **Erreur "LGHT0204"** : Vérifiez que le fichier de licence existe et est au format RTF

### Problèmes d'installation

- **Erreur 1603** : Vérifiez les journaux d'installation dans %TEMP%
- **Erreur 1612** : L'installateur est déjà en cours d'exécution
- **Erreur 1638** : Une version plus récente est déjà installée
