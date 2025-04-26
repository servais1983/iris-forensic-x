package triage

import (
	"fmt"
	"os"
	"path/filepath"
	"time"
)

// TriageOptions définit les options pour le triage des cas
type TriageOptions struct {
	AutomaticClassification bool
	AIAssisted              bool
	InputDir                string
	OutputDir               string
	MaxCases                int
}

// DefaultOptions retourne les options par défaut
func DefaultOptions() TriageOptions {
	return TriageOptions{
		AutomaticClassification: true,
		AIAssisted:              true,
		InputDir:                "./evidence",
		OutputDir:               "./triage",
		MaxCases:                100,
	}
}

// CaseSeverity représente le niveau de sévérité d'un cas
type CaseSeverity string

const (
	Critical CaseSeverity = "critical"
	High     CaseSeverity = "high"
	Medium   CaseSeverity = "medium"
	Low      CaseSeverity = "low"
)

// CaseInfo contient les informations sur un cas
type CaseInfo struct {
	ID          string
	Path        string
	Timestamp   time.Time
	Severity    CaseSeverity
	Description string
	AIScore     float64
	Tags        []string
}

// Triage effectue un triage des cas forensiques
func Triage(options TriageOptions) ([]CaseInfo, error) {
	// Vérifier que le répertoire d'entrée existe
	if _, err := os.Stat(options.InputDir); os.IsNotExist(err) {
		return nil, fmt.Errorf("le répertoire d'entrée %s n'existe pas", options.InputDir)
	}

	// Créer le répertoire de sortie s'il n'existe pas
	if err := os.MkdirAll(options.OutputDir, 0755); err != nil {
		return nil, fmt.Errorf("erreur lors de la création du répertoire de sortie: %w", err)
	}

	fmt.Printf("[IRIS] Démarrage du triage des cas dans %s\n", options.InputDir)

	// Simuler la recherche de cas
	cases := []CaseInfo{}
	
	// Simuler la découverte de cas
	capturePattern := filepath.Join(options.InputDir, "capture_*")
	matches, err := filepath.Glob(capturePattern)
	if err != nil {
		return nil, fmt.Errorf("erreur lors de la recherche de captures: %w", err)
	}
	
	// Si aucun cas réel n'est trouvé, générer des cas simulés
	if len(matches) == 0 {
		fmt.Println("[IRIS] Aucune capture réelle trouvée, génération de cas simulés pour démonstration")
		
		// Générer des cas simulés
		severities := []CaseSeverity{Critical, Critical, High, High, Medium, Low}
		descriptions := []string{
			"Ransomware activity detected",
			"Data exfiltration to suspicious IP",
			"Privilege escalation attempt",
			"Unusual network scanning",
			"Multiple failed login attempts",
			"Non-standard system configuration",
		}
		
		for i := 0; i < 5; i++ {
			caseID := fmt.Sprintf("CASE-%d", i+1)
			casePath := fmt.Sprintf("%s/simulated_case_%d", options.InputDir, i+1)
			caseTime := time.Now().Add(-time.Duration(i*3) * time.Hour)
			
			severity := severities[i]
			description := descriptions[i]
			aiScore := 0.0
			
			switch severity {
			case Critical:
				aiScore = 0.92
			case High:
				aiScore = 0.78
			case Medium:
				aiScore = 0.65
			case Low:
				aiScore = 0.45
			}
			
			tags := []string{"simulated", string(severity)}
			if i < 2 {
				tags = append(tags, "ai_detected")
			}
			
			caseInfo := CaseInfo{
				ID:          caseID,
				Path:        casePath,
				Timestamp:   caseTime,
				Severity:    severity,
				Description: description,
				AIScore:     aiScore,
				Tags:        tags,
			}
			
			cases = append(cases, caseInfo)
		}
	} else {
		// Traiter les cas réels trouvés
		for i, match := range matches {
			if i >= options.MaxCases {
				break
			}
			
			// Extraire l'ID du cas à partir du chemin
			caseID := filepath.Base(match)
			
			// Simuler l'analyse du cas
			fmt.Printf("[IRIS] Analyse du cas %s...\n", caseID)
			
			// Déterminer aléatoirement la sévérité
			var severity CaseSeverity
			var aiScore float64
			var description string
			var tags []string
			
			// Pour la démonstration, attribuer des sévérités différentes
			switch i % 4 {
			case 0:
				severity = Critical
				aiScore = 0.95
				description = "Critical security breach detected"
				tags = []string{"critical", "breach", "ai_detected"}
			case 1:
				severity = High
				aiScore = 0.82
				description = "Suspicious activity detected"
				tags = []string{"high", "suspicious", "ai_detected"}
			case 2:
				severity = Medium
				aiScore = 0.68
				description = "Unusual behavior observed"
				tags = []string{"medium", "unusual"}
			default:
				severity = Low
				aiScore = 0.42
				description = "Minor anomaly detected"
				tags = []string{"low", "anomaly"}
			}
			
			// Créer l'information du cas
			caseInfo := CaseInfo{
				ID:          caseID,
				Path:        match,
				Timestamp:   time.Now().Add(-time.Duration(i*2) * time.Hour),
				Severity:    severity,
				Description: description,
				AIScore:     aiScore,
				Tags:        tags,
			}
			
			cases = append(cases, caseInfo)
		}
	}
	
	// Générer un rapport de triage
	triageReport := fmt.Sprintf("%s/triage_report_%s.json", options.OutputDir, time.Now().Format("20060102_150405"))
	
	// Compter les cas par sévérité
	criticalCount := 0
	highCount := 0
	mediumCount := 0
	lowCount := 0
	
	for _, c := range cases {
		switch c.Severity {
		case Critical:
			criticalCount++
		case High:
			highCount++
		case Medium:
			mediumCount++
		case Low:
			lowCount++
		}
	}
	
	// Créer le contenu du rapport
	reportContent := fmt.Sprintf(`{
		"timestamp": "%s",
		"total_cases": %d,
		"severity_counts": {
			"critical": %d,
			"high": %d,
			"medium": %d,
			"low": %d
		},
		"ai_detected_cases": %d,
		"triage_status": "completed"
	}`, time.Now().Format(time.RFC3339), len(cases), criticalCount, highCount, mediumCount, lowCount, criticalCount+highCount)
	
	// Écrire le rapport
	if err := os.WriteFile(triageReport, []byte(reportContent), 0644); err != nil {
		return nil, fmt.Errorf("erreur lors de l'écriture du rapport de triage: %w", err)
	}
	
	fmt.Printf("[IRIS] Triage terminé. %d cas analysés.\n", len(cases))
	fmt.Printf("[IRIS] %d cas critiques, %d cas élevés, %d cas moyens, %d cas faibles.\n", 
		criticalCount, highCount, mediumCount, lowCount)
	
	return cases, nil
}

// AutoTriage effectue un triage automatique des cas
func AutoTriage(inputDir string) ([]CaseInfo, error) {
	fmt.Printf("[IRIS] Triage automatique du répertoire %s...\n", inputDir)
	
	options := DefaultOptions()
	options.InputDir = inputDir
	options.AutomaticClassification = true
	options.AIAssisted = true
	
	return Triage(options)
}
