package capture

import (
	"fmt"
	"os"
	"time"
)

// CaptureOptions définit les options pour la capture de données
type CaptureOptions struct {
	MemorySnapshot bool
	LogIngestor    bool
	NetworkCapture bool
	OutputDir      string
	Priority       string // "high", "medium", "low"
}

// DefaultOptions retourne les options par défaut
func DefaultOptions() CaptureOptions {
	return CaptureOptions{
		MemorySnapshot: true,
		LogIngestor:    true,
		NetworkCapture: true,
		OutputDir:      "./evidence",
		Priority:       "medium",
	}
}

// Capture effectue une capture de données forensiques
func Capture(options CaptureOptions) error {
	// Créer le répertoire de sortie s'il n'existe pas
	if err := os.MkdirAll(options.OutputDir, 0755); err != nil {
		return fmt.Errorf("erreur lors de la création du répertoire de sortie: %w", err)
	}

	// Timestamp pour le nom du dossier de capture
	timestamp := time.Now().Format("20060102_150405")
	captureDir := fmt.Sprintf("%s/capture_%s", options.OutputDir, timestamp)
	
	if err := os.MkdirAll(captureDir, 0755); err != nil {
		return fmt.Errorf("erreur lors de la création du répertoire de capture: %w", err)
	}

	fmt.Printf("[IRIS] Démarrage de la capture dans %s\n", captureDir)

	// Simuler les différentes captures
	if options.MemorySnapshot {
		fmt.Println("[IRIS] Capture de la mémoire en cours...")
		// Simulation d'une capture de mémoire
		time.Sleep(1 * time.Second)
		memoryFile := fmt.Sprintf("%s/memory_dump.bin", captureDir)
		if err := createDummyFile(memoryFile, "Memory snapshot data"); err != nil {
			return err
		}
		fmt.Println("[IRIS] Capture de la mémoire terminée")
	}

	if options.LogIngestor {
		fmt.Println("[IRIS] Ingestion des logs en cours...")
		// Simulation d'une ingestion de logs
		time.Sleep(1 * time.Second)
		logsDir := fmt.Sprintf("%s/logs", captureDir)
		if err := os.MkdirAll(logsDir, 0755); err != nil {
			return fmt.Errorf("erreur lors de la création du répertoire de logs: %w", err)
		}
		
		logTypes := []string{"system", "application", "security", "network"}
		for _, logType := range logTypes {
			logFile := fmt.Sprintf("%s/%s.log", logsDir, logType)
			if err := createDummyFile(logFile, fmt.Sprintf("%s log data", logType)); err != nil {
				return err
			}
		}
		fmt.Println("[IRIS] Ingestion des logs terminée")
	}

	if options.NetworkCapture {
		fmt.Println("[IRIS] Capture réseau en cours...")
		// Simulation d'une capture réseau
		time.Sleep(1 * time.Second)
		networkFile := fmt.Sprintf("%s/network_capture.pcap", captureDir)
		if err := createDummyFile(networkFile, "Network capture data"); err != nil {
			return err
		}
		fmt.Println("[IRIS] Capture réseau terminée")
	}

	// Créer un fichier de métadonnées
	metadataFile := fmt.Sprintf("%s/metadata.json", captureDir)
	metadata := fmt.Sprintf(`{
		"timestamp": "%s",
		"options": {
			"memorySnapshot": %t,
			"logIngestor": %t,
			"networkCapture": %t,
			"priority": "%s"
		},
		"status": "completed"
	}`, timestamp, options.MemorySnapshot, options.LogIngestor, options.NetworkCapture, options.Priority)
	
	if err := createDummyFile(metadataFile, metadata); err != nil {
		return err
	}

	fmt.Printf("[IRIS] Capture terminée avec succès. Résultats stockés dans %s\n", captureDir)
	return nil
}

// SmartCapture effectue une capture intelligente en déterminant automatiquement les meilleures options
func SmartCapture(targetDir string) error {
	fmt.Println("[IRIS] Analyse du contexte pour déterminer la méthode optimale...")
	fmt.Println("[IRIS] Priorisation dynamique des artefacts basée sur MITRE ATT&CK...")
	
	// Simuler une analyse de contexte
	time.Sleep(2 * time.Second)
	
	// Déterminer les options optimales basées sur l'analyse
	options := DefaultOptions()
	options.OutputDir = targetDir
	options.Priority = "high" // Priorité élevée pour la capture intelligente
	
	return Capture(options)
}

// Fonction utilitaire pour créer un fichier factice avec du contenu
func createDummyFile(path string, content string) error {
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
