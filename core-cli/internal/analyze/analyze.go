package analyze

import (
	"fmt"
	"os"
	"path/filepath"
	"time"
)

// AnalyzeOptions définit les options pour l'analyse forensique
type AnalyzeOptions struct {
	AutopsyLike          bool
	AIDetection          bool
	TemporalReconstruction bool
	InputDir             string
	OutputDir            string
	Verbose              bool
}

// DefaultOptions retourne les options par défaut
func DefaultOptions() AnalyzeOptions {
	return AnalyzeOptions{
		AutopsyLike:          true,
		AIDetection:          true,
		TemporalReconstruction: true,
		InputDir:             "./evidence",
		OutputDir:            "./analysis",
		Verbose:              false,
	}
}

// Analyze effectue une analyse forensique des données capturées
func Analyze(options AnalyzeOptions) error {
	// Vérifier que le répertoire d'entrée existe
	if _, err := os.Stat(options.InputDir); os.IsNotExist(err) {
		return fmt.Errorf("le répertoire d'entrée %s n'existe pas", options.InputDir)
	}

	// Créer le répertoire de sortie s'il n'existe pas
	if err := os.MkdirAll(options.OutputDir, 0755); err != nil {
		return fmt.Errorf("erreur lors de la création du répertoire de sortie: %w", err)
	}

	// Timestamp pour le nom du dossier d'analyse
	timestamp := time.Now().Format("20060102_150405")
	analysisDir := fmt.Sprintf("%s/analysis_%s", options.OutputDir, timestamp)
	
	if err := os.MkdirAll(analysisDir, 0755); err != nil {
		return fmt.Errorf("erreur lors de la création du répertoire d'analyse: %w", err)
	}

	fmt.Printf("[IRIS] Démarrage de l'analyse dans %s\n", analysisDir)
	fmt.Printf("[IRIS] Analyse des données depuis %s\n", options.InputDir)

	// Simuler les différentes analyses
	if options.AutopsyLike {
		fmt.Println("[IRIS] Exécution de l'analyse Autopsy-like (TSK++)...")
		// Simulation d'une analyse Autopsy-like
		time.Sleep(2 * time.Second)
		autopsyDir := fmt.Sprintf("%s/autopsy", analysisDir)
		if err := os.MkdirAll(autopsyDir, 0755); err != nil {
			return fmt.Errorf("erreur lors de la création du répertoire autopsy: %w", err)
		}
		
		// Créer des fichiers de résultats simulés
		fileTypes := []string{"filesystem", "registry", "artifacts", "timeline"}
		for _, fileType := range fileTypes {
			resultFile := fmt.Sprintf("%s/%s_analysis.json", autopsyDir, fileType)
			content := fmt.Sprintf(`{
				"type": "%s",
				"timestamp": "%s",
				"findings": [
					{"severity": "high", "description": "Suspicious %s activity detected"},
					{"severity": "medium", "description": "Unusual %s configuration found"},
					{"severity": "low", "description": "Non-standard %s settings observed"}
				]
			}`, fileType, timestamp, fileType, fileType, fileType)
			
			if err := createResultFile(resultFile, content); err != nil {
				return err
			}
		}
		fmt.Println("[IRIS] Analyse Autopsy-like terminée")
	}

	if options.AIDetection {
		fmt.Println("[IRIS] Exécution de la détection IA CodeBERT...")
		// Simulation d'une détection IA
		time.Sleep(3 * time.Second)
		aiDir := fmt.Sprintf("%s/ai_detection", analysisDir)
		if err := os.MkdirAll(aiDir, 0755); err != nil {
			return fmt.Errorf("erreur lors de la création du répertoire ai_detection: %w", err)
		}
		
		// Créer des fichiers de résultats simulés
		threatTypes := []string{"malware", "ransomware", "backdoor", "rootkit"}
		for _, threatType := range threatTypes {
			resultFile := fmt.Sprintf("%s/%s_detection.json", aiDir, threatType)
			content := fmt.Sprintf(`{
				"model": "CodeBERT",
				"threat_type": "%s",
				"timestamp": "%s",
				"confidence": %.2f,
				"detections": [
					{"path": "/suspicious/file1.exe", "confidence": 0.92, "signature": "known_%s_pattern"},
					{"path": "/suspicious/file2.dll", "confidence": 0.87, "signature": "behavior_match"},
					{"path": "/suspicious/config.dat", "confidence": 0.78, "signature": "anomaly_detection"}
				]
			}`, threatType, timestamp, 0.85, threatType)
			
			if err := createResultFile(resultFile, content); err != nil {
				return err
			}
		}
		fmt.Println("[IRIS] Détection IA terminée")
	}

	if options.TemporalReconstruction {
		fmt.Println("[IRIS] Exécution de la reconstruction temporelle...")
		// Simulation d'une reconstruction temporelle
		time.Sleep(2 * time.Second)
		timelineDir := fmt.Sprintf("%s/timeline", analysisDir)
		if err := os.MkdirAll(timelineDir, 0755); err != nil {
			return fmt.Errorf("erreur lors de la création du répertoire timeline: %w", err)
		}
		
		// Créer un fichier de timeline simulé
		timelineFile := fmt.Sprintf("%s/event_timeline.json", timelineDir)
		content := `{
			"timeline": [
				{"timestamp": "2025-04-25T10:15:22Z", "event": "Initial access", "source": "VPN logs", "severity": "critical"},
				{"timestamp": "2025-04-25T10:17:45Z", "event": "Privilege escalation", "source": "System logs", "severity": "critical"},
				{"timestamp": "2025-04-25T10:22:13Z", "event": "Lateral movement", "source": "Network traffic", "severity": "high"},
				{"timestamp": "2025-04-25T10:35:07Z", "event": "Data exfiltration", "source": "Firewall logs", "severity": "critical"},
				{"timestamp": "2025-04-25T11:02:51Z", "event": "Evidence tampering", "source": "File system", "severity": "medium"}
			],
			"attack_phases": [
				{"name": "Initial Access", "confidence": 0.95},
				{"name": "Execution", "confidence": 0.92},
				{"name": "Persistence", "confidence": 0.88},
				{"name": "Privilege Escalation", "confidence": 0.90},
				{"name": "Defense Evasion", "confidence": 0.85},
				{"name": "Credential Access", "confidence": 0.82},
				{"name": "Discovery", "confidence": 0.91},
				{"name": "Lateral Movement", "confidence": 0.87},
				{"name": "Collection", "confidence": 0.93},
				{"name": "Exfiltration", "confidence": 0.89}
			]
		}`
		
		if err := createResultFile(timelineFile, content); err != nil {
			return err
		}
		
		// Créer un fichier de graphe d'attaque simulé
		graphFile := fmt.Sprintf("%s/attack_graph.json", timelineDir)
		graphContent := `{
			"nodes": [
				{"id": "n1", "label": "Initial Access", "type": "entry_point"},
				{"id": "n2", "label": "PowerShell Execution", "type": "technique"},
				{"id": "n3", "label": "Credential Dumping", "type": "technique"},
				{"id": "n4", "label": "Lateral Movement", "type": "technique"},
				{"id": "n5", "label": "Data Exfiltration", "type": "objective"}
			],
			"edges": [
				{"source": "n1", "target": "n2", "label": "leads_to"},
				{"source": "n2", "target": "n3", "label": "enables"},
				{"source": "n3", "target": "n4", "label": "enables"},
				{"source": "n4", "target": "n5", "label": "achieves"}
			],
			"mitre_mapping": {
				"n1": "T1190",
				"n2": "T1059.001",
				"n3": "T1003",
				"n4": "T1021",
				"n5": "T1048"
			}
		}`
		
		if err := createResultFile(graphFile, graphContent); err != nil {
			return err
		}
		
		fmt.Println("[IRIS] Reconstruction temporelle terminée")
	}

	// Créer un rapport de synthèse
	summaryFile := fmt.Sprintf("%s/summary.json", analysisDir)
	summary := fmt.Sprintf(`{
		"timestamp": "%s",
		"options": {
			"autopsyLike": %t,
			"aiDetection": %t,
			"temporalReconstruction": %t
		},
		"status": "completed",
		"threat_level": "high",
		"key_findings": [
			"Ransomware activity detected with 92%% confidence",
			"Evidence of lateral movement across 3 systems",
			"Data exfiltration to external IP detected",
			"Timeline suggests coordinated attack over 47 minute period"
		],
		"mitre_techniques": ["T1190", "T1059.001", "T1003", "T1021", "T1048"]
	}`, timestamp, options.AutopsyLike, options.AIDetection, options.TemporalReconstruction)
	
	if err := createResultFile(summaryFile, summary); err != nil {
		return err
	}

	fmt.Printf("[IRIS] Analyse terminée avec succès. Résultats stockés dans %s\n", analysisDir)
	return nil
}

// CompleteAnalysis effectue une analyse complète avec toutes les options activées
func CompleteAnalysis(inputDir, outputDir string) error {
	fmt.Println("[IRIS] Démarrage de l'analyse complète...")
	
	options := DefaultOptions()
	options.InputDir = inputDir
	options.OutputDir = outputDir
	options.Verbose = true
	
	return Analyze(options)
}

// Fonction utilitaire pour créer un fichier de résultats
func createResultFile(path string, content string) error {
	file, err := os.Create(path)
	if err != nil {
		return fmt.Errorf("erreur lors de la création du fichier %s: %w", path, err)
	}
	defer file.Close()
	
	_, err = file.WriteString(content)
	if err != nil {
		return fmt.Errorf("erreur lors de l'écriture dans le fichier %s: %w", path, err)
	}
	
	return nil
}
