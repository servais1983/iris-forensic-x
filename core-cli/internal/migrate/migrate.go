package migrate

import (
	"fmt"
	"os"
	"path/filepath"
	"time"
)

// MigrationOptions définit les options pour la migration des données
type MigrationOptions struct {
	SourceVersion string
	TargetVersion string
	SourceDir     string
	TargetDir     string
	PreserveData  bool
	ConvertConfig bool
}

// DefaultOptions retourne les options par défaut
func DefaultOptions() MigrationOptions {
	return MigrationOptions{
		SourceVersion: "v1",
		TargetVersion: "X",
		SourceDir:     "./data_v1",
		TargetDir:     "./data",
		PreserveData:  true,
		ConvertConfig: true,
	}
}

// MigrationResult contient les informations sur une migration
type MigrationResult struct {
	StartTime      time.Time
	EndTime        time.Time
	SourceVersion  string
	TargetVersion  string
	ItemsMigrated  int
	Status         string // "success", "failed", "partial"
	ErrorMessages  []string
	WarningMessages []string
}

// Migrate effectue une migration des données d'une version à une autre
func Migrate(options MigrationOptions) (*MigrationResult, error) {
	// Vérifier que le répertoire source existe
	if _, err := os.Stat(options.SourceDir); os.IsNotExist(err) {
		return nil, fmt.Errorf("le répertoire source %s n'existe pas", options.SourceDir)
	}

	// Créer le répertoire cible s'il n'existe pas
	if err := os.MkdirAll(options.TargetDir, 0755); err != nil {
		return nil, fmt.Errorf("erreur lors de la création du répertoire cible: %w", err)
	}

	fmt.Printf("[IRIS] Démarrage de la migration depuis %s vers %s\n", options.SourceVersion, options.TargetVersion)
	fmt.Printf("[IRIS] Source: %s, Cible: %s\n", options.SourceDir, options.TargetDir)

	// Initialiser le résultat de la migration
	result := &MigrationResult{
		StartTime:      time.Now(),
		SourceVersion:  options.SourceVersion,
		TargetVersion:  options.TargetVersion,
		ItemsMigrated:  0,
		Status:         "in_progress",
		ErrorMessages:  []string{},
		WarningMessages: []string{},
	}

	// Simuler la migration des données
	fmt.Println("[IRIS] Migration des données en cours...")
	time.Sleep(1 * time.Second)

	// Simuler la migration des cas
	casesDir := filepath.Join(options.TargetDir, "cases")
	if err := os.MkdirAll(casesDir, 0755); err != nil {
		result.Status = "failed"
		result.ErrorMessages = append(result.ErrorMessages, fmt.Sprintf("Erreur lors de la création du répertoire des cas: %v", err))
		result.EndTime = time.Now()
		return result, err
	}

	// Simuler la migration des configurations
	if options.ConvertConfig {
		fmt.Println("[IRIS] Conversion des configurations en cours...")
		configDir := filepath.Join(options.TargetDir, "config")
		if err := os.MkdirAll(configDir, 0755); err != nil {
			result.Status = "partial"
			result.ErrorMessages = append(result.ErrorMessages, fmt.Sprintf("Erreur lors de la création du répertoire de configuration: %v", err))
		} else {
			// Créer un fichier de configuration simulé
			configFile := filepath.Join(configDir, "config.json")
			configContent := fmt.Sprintf(`{
				"version": "%s",
				"migrated_from": "%s",
				"migration_date": "%s",
				"settings": {
					"capture": {
						"memory_snapshot": true,
						"log_ingestor": true,
						"network_capture": true
					},
					"analyze": {
						"autopsy_like": true,
						"ai_detection": true,
						"temporal_reconstruction": true
					},
					"triage": {
						"automatic_classification": true,
						"ai_assisted": true
					},
					"respond": {
						"automatic_response": false,
						"isolation_enabled": true,
						"evidence_sealing": true,
						"report_generation": true
					}
				}
			}`, options.TargetVersion, options.SourceVersion, time.Now().Format(time.RFC3339))
			
			if err := os.WriteFile(configFile, []byte(configContent), 0644); err != nil {
				result.WarningMessages = append(result.WarningMessages, fmt.Sprintf("Erreur lors de l'écriture du fichier de configuration: %v", err))
			} else {
				result.ItemsMigrated++
			}
		}
	}

	// Simuler la migration des modèles ML
	fmt.Println("[IRIS] Migration des modèles ML en cours...")
	modelsDir := filepath.Join(options.TargetDir, "models")
	if err := os.MkdirAll(modelsDir, 0755); err != nil {
		result.Status = "partial"
		result.ErrorMessages = append(result.ErrorMessages, fmt.Sprintf("Erreur lors de la création du répertoire des modèles: %v", err))
	} else {
		// Créer un fichier de métadonnées de modèle simulé
		modelMetaFile := filepath.Join(modelsDir, "model_metadata.json")
		modelMetaContent := fmt.Sprintf(`{
			"models": [
				{
					"name": "ransomware_detector",
					"version": "1.2.0",
					"migrated_from": "%s",
					"compatible_with": "%s",
					"accuracy": 0.92,
					"last_trained": "%s"
				},
				{
					"name": "malware_classifier",
					"version": "2.1.0",
					"migrated_from": "%s",
					"compatible_with": "%s",
					"accuracy": 0.89,
					"last_trained": "%s"
				},
				{
					"name": "timeline_reconstructor",
					"version": "1.0.0",
					"migrated_from": "%s",
					"compatible_with": "%s",
					"accuracy": 0.85,
					"last_trained": "%s"
				}
			]
		}`, 
			options.SourceVersion, options.TargetVersion, time.Now().AddDate(0, -1, 0).Format(time.RFC3339),
			options.SourceVersion, options.TargetVersion, time.Now().AddDate(0, -2, 0).Format(time.RFC3339),
			options.SourceVersion, options.TargetVersion, time.Now().AddDate(0, -3, 0).Format(time.RFC3339),
		)
		
		if err := os.WriteFile(modelMetaFile, []byte(modelMetaContent), 0644); err != nil {
			result.WarningMessages = append(result.WarningMessages, fmt.Sprintf("Erreur lors de l'écriture du fichier de métadonnées des modèles: %v", err))
		} else {
			result.ItemsMigrated++
		}
	}

	// Simuler la migration des plugins
	fmt.Println("[IRIS] Migration des plugins en cours...")
	pluginsDir := filepath.Join(options.TargetDir, "plugins")
	if err := os.MkdirAll(pluginsDir, 0755); err != nil {
		result.Status = "partial"
		result.ErrorMessages = append(result.ErrorMessages, fmt.Sprintf("Erreur lors de la création du répertoire des plugins: %v", err))
	} else {
		// Créer un fichier de métadonnées de plugin simulé
		pluginMetaFile := filepath.Join(pluginsDir, "plugin_metadata.json")
		pluginMetaContent := fmt.Sprintf(`{
			"plugins": [
				{
					"name": "cloud_analyzer",
					"version": "1.0.0",
					"migrated_from": "%s",
					"compatible_with": "%s",
					"author": "IRIS Team",
					"description": "Analyze cloud infrastructure and logs"
				},
				{
					"name": "memory_forensics",
					"version": "2.1.0",
					"migrated_from": "%s",
					"compatible_with": "%s",
					"author": "IRIS Team",
					"description": "Advanced memory forensics capabilities"
				},
				{
					"name": "report_generator",
					"version": "1.5.0",
					"migrated_from": "%s",
					"compatible_with": "%s",
					"author": "IRIS Team",
					"description": "Generate comprehensive forensic reports"
				}
			]
		}`, 
			options.SourceVersion, options.TargetVersion,
			options.SourceVersion, options.TargetVersion,
			options.SourceVersion, options.TargetVersion,
		)
		
		if err := os.WriteFile(pluginMetaFile, []byte(pluginMetaContent), 0644); err != nil {
			result.WarningMessages = append(result.WarningMessages, fmt.Sprintf("Erreur lors de l'écriture du fichier de métadonnées des plugins: %v", err))
		} else {
			result.ItemsMigrated++
		}
	}

	// Finaliser la migration
	if len(result.ErrorMessages) == 0 {
		result.Status = "success"
	} else if result.Status != "failed" {
		result.Status = "partial"
	}

	result.EndTime = time.Now()

	// Créer un rapport de migration
	migrationReportPath := filepath.Join(options.TargetDir, "migration_report.json")
	migrationReport := fmt.Sprintf(`{
		"source_version": "%s",
		"target_version": "%s",
		"start_time": "%s",
		"end_time": "%s",
		"items_migrated": %d,
		"status": "%s",
		"errors": %s,
		"warnings": %s
	}`, 
		result.SourceVersion,
		result.TargetVersion,
		result.StartTime.Format(time.RFC3339),
		result.EndTime.Format(time.RFC3339),
		result.ItemsMigrated,
		result.Status,
		formatStringArrayJSON(result.ErrorMessages),
		formatStringArrayJSON(result.WarningMessages),
	)
	
	if err := os.WriteFile(migrationReportPath, []byte(migrationReport), 0644); err != nil {
		fmt.Printf("[IRIS] Avertissement: Impossible d'écrire le rapport de migration: %v\n", err)
	}

	fmt.Printf("[IRIS] Migration terminée avec le statut: %s\n", result.Status)
	fmt.Printf("[IRIS] %d éléments migrés\n", result.ItemsMigrated)
	
	if len(result.ErrorMessages) > 0 {
		fmt.Println("[IRIS] Erreurs rencontrées:")
		for _, errMsg := range result.ErrorMessages {
			fmt.Printf("  - %s\n", errMsg)
		}
	}
	
	if len(result.WarningMessages) > 0 {
		fmt.Println("[IRIS] Avertissements:")
		for _, warnMsg := range result.WarningMessages {
			fmt.Printf("  - %s\n", warnMsg)
		}
	}
	
	return result, nil
}

// MigrateFromTo effectue une migration entre deux versions spécifiques
func MigrateFromTo(from, to, sourceDir, targetDir string) (*MigrationResult, error) {
	fmt.Printf("[IRIS] Migration des données depuis %s vers %s...\n", from, to)
	
	options := DefaultOptions()
	options.SourceVersion = from
	options.TargetVersion = to
	options.SourceDir = sourceDir
	options.TargetDir = targetDir
	
	return Migrate(options)
}

// Fonction utilitaire pour formater un tableau de chaînes en JSON
func formatStringArrayJSON(arr []string) string {
	if len(arr) == 0 {
		return "[]"
	}
	
	result := "["
	for i, str := range arr {
		if i > 0 {
			result += ", "
		}
		result += fmt.Sprintf(`"%s"`, str)
	}
	result += "]"
	
	return result
}
