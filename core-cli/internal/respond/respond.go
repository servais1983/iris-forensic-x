package respond

import (
	"fmt"
	"os"
	"path/filepath"
	"time"
)

// ResponseOptions définit les options pour la réponse aux incidents
type ResponseOptions struct {
	AutomaticResponse bool
	IsolationEnabled  bool
	EvidenceSealing   bool
	ReportGeneration  bool
	RuleSet           string
	CaseID            string
	OutputDir         string
}

// DefaultOptions retourne les options par défaut
func DefaultOptions() ResponseOptions {
	return ResponseOptions{
		AutomaticResponse: false,
		IsolationEnabled:  true,
		EvidenceSealing:   true,
		ReportGeneration:  true,
		RuleSet:           "standard",
		CaseID:            "",
		OutputDir:         "./response",
	}
}

// ResponseAction représente une action de réponse à un incident
type ResponseAction struct {
	Type        string    // "isolation", "evidence_sealing", "report", etc.
	Timestamp   time.Time
	Description string
	Status      string // "success", "failed", "pending"
	Details     map[string]string
}

// IncidentResponse contient les informations sur une réponse à un incident
type IncidentResponse struct {
	CaseID      string
	StartTime   time.Time
	EndTime     time.Time
	RuleSet     string
	Actions     []ResponseAction
	Status      string // "completed", "in_progress", "failed"
	ReportPath  string
}

// Respond effectue une réponse à un incident
func Respond(options ResponseOptions) (*IncidentResponse, error) {
	// Créer le répertoire de sortie s'il n'existe pas
	if err := os.MkdirAll(options.OutputDir, 0755); err != nil {
		return nil, fmt.Errorf("erreur lors de la création du répertoire de sortie: %w", err)
	}

	// Générer un ID de cas si non spécifié
	caseID := options.CaseID
	if caseID == "" {
		caseID = fmt.Sprintf("RESP-%s", time.Now().Format("20060102-150405"))
	}

	// Créer un répertoire pour ce cas
	caseDir := filepath.Join(options.OutputDir, caseID)
	if err := os.MkdirAll(caseDir, 0755); err != nil {
		return nil, fmt.Errorf("erreur lors de la création du répertoire du cas: %w", err)
	}

	fmt.Printf("[IRIS] Démarrage de la réponse à l'incident %s\n", caseID)
	fmt.Printf("[IRIS] Utilisation du jeu de règles: %s\n", options.RuleSet)

	// Initialiser la réponse à l'incident
	response := &IncidentResponse{
		CaseID:    caseID,
		StartTime: time.Now(),
		RuleSet:   options.RuleSet,
		Actions:   []ResponseAction{},
		Status:    "in_progress",
	}

	// Exécuter les actions de réponse
	if options.IsolationEnabled {
		fmt.Println("[IRIS] Exécution de l'isolation...")
		action := performIsolation(caseDir, options.RuleSet)
		response.Actions = append(response.Actions, action)
		fmt.Printf("[IRIS] Isolation %s\n", action.Status)
	}

	if options.EvidenceSealing {
		fmt.Println("[IRIS] Exécution du scellement des preuves...")
		action := performEvidenceSealing(caseDir, options.RuleSet)
		response.Actions = append(response.Actions, action)
		fmt.Printf("[IRIS] Scellement des preuves %s\n", action.Status)
	}

	if options.ReportGeneration {
		fmt.Println("[IRIS] Génération du rapport d'incident...")
		action := generateIncidentReport(caseDir, caseID, options.RuleSet)
		response.Actions = append(response.Actions, action)
		response.ReportPath = action.Details["report_path"]
		fmt.Printf("[IRIS] Génération du rapport %s\n", action.Status)
	}

	// Finaliser la réponse
	response.EndTime = time.Now()
	response.Status = "completed"

	// Sauvegarder le résumé de la réponse
	summaryPath := filepath.Join(caseDir, "response_summary.json")
	summary := fmt.Sprintf(`{
		"case_id": "%s",
		"start_time": "%s",
		"end_time": "%s",
		"rule_set": "%s",
		"status": "%s",
		"actions": [
			%s
		]
	}`, 
		response.CaseID,
		response.StartTime.Format(time.RFC3339),
		response.EndTime.Format(time.RFC3339),
		response.RuleSet,
		response.Status,
		formatActionsJSON(response.Actions),
	)

	if err := os.WriteFile(summaryPath, []byte(summary), 0644); err != nil {
		return nil, fmt.Errorf("erreur lors de l'écriture du résumé de la réponse: %w", err)
	}

	fmt.Printf("[IRIS] Réponse à l'incident terminée. Résultats stockés dans %s\n", caseDir)
	return response, nil
}

// AutoRespond effectue une réponse automatique à un incident selon une règle spécifique
func AutoRespond(rule string) (*IncidentResponse, error) {
	fmt.Printf("[IRIS] Réponse automatique selon la règle %s...\n", rule)
	
	options := DefaultOptions()
	options.AutomaticResponse = true
	options.RuleSet = rule
	
	return Respond(options)
}

// Fonction simulant l'isolation d'un système compromis
func performIsolation(caseDir string, ruleSet string) ResponseAction {
	// Simuler un délai pour l'opération
	time.Sleep(1 * time.Second)
	
	// Créer un fichier de log pour l'isolation
	logPath := filepath.Join(caseDir, "isolation.log")
	logContent := fmt.Sprintf(`
Isolation Log - %s
Rule Set: %s
Timestamp: %s

1. Network isolation initiated
   - Firewall rules updated
   - External communications blocked
   - Monitoring interfaces preserved

2. Process isolation completed
   - Suspicious processes terminated
   - Auto-start entries removed
   - System services verified

3. User account containment
   - Privileged accounts secured
   - Password resets enforced
   - MFA verification enabled

Isolation Status: COMPLETE
	`, time.Now().Format(time.RFC3339), ruleSet, time.Now().Format(time.RFC3339))
	
	os.WriteFile(logPath, []byte(logContent), 0644)
	
	return ResponseAction{
		Type:        "isolation",
		Timestamp:   time.Now(),
		Description: "System isolation to prevent lateral movement",
		Status:      "success",
		Details: map[string]string{
			"log_path": logPath,
			"rule_set": ruleSet,
			"method":   "network_and_process_isolation",
		},
	}
}

// Fonction simulant le scellement des preuves
func performEvidenceSealing(caseDir string, ruleSet string) ResponseAction {
	// Simuler un délai pour l'opération
	time.Sleep(1 * time.Second)
	
	// Créer un fichier de manifeste pour les preuves
	manifestPath := filepath.Join(caseDir, "evidence_manifest.json")
	manifestContent := fmt.Sprintf(`{
		"timestamp": "%s",
		"rule_set": "%s",
		"evidence_items": [
			{
				"id": "EV-001",
				"type": "memory_dump",
				"hash": "sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855",
				"timestamp": "%s",
				"sealed": true
			},
			{
				"id": "EV-002",
				"type": "network_capture",
				"hash": "sha256:a7ffc6f8bf1ed76651c14756a061d662f580ff4de43b49fa82d80a4b80f8434a",
				"timestamp": "%s",
				"sealed": true
			},
			{
				"id": "EV-003",
				"type": "log_archive",
				"hash": "sha256:3fdba35f04dc8c462986c992bcf875546257113072a909c162f7e470e581e278",
				"timestamp": "%s",
				"sealed": true
			}
		],
		"blockchain_receipt": {
			"transaction_id": "0x7f9fade1c0d57a7af66ab4ead79fade1c0d57a7af66ab4ead7acad1c0d57a7af",
			"timestamp": "%s",
			"network": "Hyperledger Fabric",
			"verification_url": "https://verify.iris-forensic.com/evidence/0x7f9fade1c0d57a7af66ab4ead79fade1c0d57a7af66ab4ead7acad1c0d57a7af"
		}
	}`, 
		time.Now().Format(time.RFC3339),
		ruleSet,
		time.Now().Add(-30 * time.Minute).Format(time.RFC3339),
		time.Now().Add(-25 * time.Minute).Format(time.RFC3339),
		time.Now().Add(-20 * time.Minute).Format(time.RFC3339),
		time.Now().Format(time.RFC3339),
	)
	
	os.WriteFile(manifestPath, []byte(manifestContent), 0644)
	
	return ResponseAction{
		Type:        "evidence_sealing",
		Timestamp:   time.Now(),
		Description: "Cryptographic sealing of evidence with blockchain verification",
		Status:      "success",
		Details: map[string]string{
			"manifest_path": manifestPath,
			"items_sealed":  "3",
			"blockchain":    "Hyperledger Fabric",
		},
	}
}

// Fonction simulant la génération d'un rapport d'incident
func generateIncidentReport(caseDir string, caseID string, ruleSet string) ResponseAction {
	// Simuler un délai pour l'opération
	time.Sleep(2 * time.Second)
	
	// Créer un rapport d'incident
	reportPath := filepath.Join(caseDir, "incident_report.html")
	reportContent := fmt.Sprintf(`<!DOCTYPE html>
<html>
<head>
    <title>Incident Response Report - %s</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        h1 { color: #2c3e50; border-bottom: 1px solid #eee; padding-bottom: 10px; }
        h2 { color: #3498db; margin-top: 30px; }
        .critical { color: #e74c3c; }
        .high { color: #e67e22; }
        .medium { color: #f39c12; }
        .low { color: #2ecc71; }
        .metadata { background-color: #f8f9fa; padding: 15px; border-radius: 5px; }
        table { width: 100%%; border-collapse: collapse; margin: 20px 0; }
        table, th, td { border: 1px solid #ddd; }
        th, td { padding: 12px; text-align: left; }
        th { background-color: #f2f2f2; }
    </style>
</head>
<body>
    <h1>Incident Response Report</h1>
    
    <div class="metadata">
        <p><strong>Case ID:</strong> %s</p>
        <p><strong>Date:</strong> %s</p>
        <p><strong>Rule Set:</strong> %s</p>
        <p><strong>Status:</strong> Completed</p>
    </div>
    
    <h2>Executive Summary</h2>
    <p>
        A security incident was detected and responded to according to the %s ruleset.
        The incident involved a suspected ransomware attack with data exfiltration attempts.
        The response team successfully isolated affected systems, secured evidence, and prevented further compromise.
    </p>
    
    <h2>Incident Timeline</h2>
    <table>
        <tr>
            <th>Timestamp</th>
            <th>Event</th>
            <th>Severity</th>
        </tr>
        <tr>
            <td>%s</td>
            <td>Initial detection of suspicious activity</td>
            <td class="medium">Medium</td>
        </tr>
        <tr>
            <td>%s</td>
            <td>Confirmed malicious behavior - Ransomware indicators identified</td>
            <td class="critical">Critical</td>
        </tr>
        <tr>
            <td>%s</td>
            <td>Response team engaged</td>
            <td class="high">High</td>
        </tr>
        <tr>
            <td>%s</td>
            <td>Systems isolated</td>
            <td class="high">High</td>
        </tr>
        <tr>
            <td>%s</td>
            <td>Evidence collection completed</td>
            <td class="medium">Medium</td>
        </tr>
        <tr>
            <td>%s</td>
            <td>Forensic analysis initiated</td>
            <td class="medium">Medium</td>
        </tr>
        <tr>
            <td>%s</td>
            <td>Incident contained</td>
            <td class="high">High</td>
        </tr>
    </table>
    
    <h2>Actions Taken</h2>
    <ul>
        <li>Network isolation of affected systems</li>
        <li>Memory dumps collected from 3 critical systems</li>
        <li>Network traffic captured and analyzed</li>
        <li>Log files secured and sealed with blockchain verification</li>
        <li>Malicious processes terminated</li>
        <li>Compromised accounts secured</li>
    </ul>
    
    <h2>Findings</h2>
    <p>
        The incident analysis revealed a targeted ransomware attack using the LockBit 3.0 variant.
        The initial access vector appears to be a phishing email with a malicious attachment.
        The attacker established persistence through scheduled tasks and modified registry keys.
        Data exfiltration was attempted but blocked by the response actions.
    </p>
    
    <h2>MITRE ATT&CK Techniques Identified</h2>
    <ul>
        <li>T1566.001 - Phishing: Spearphishing Attachment</li>
        <li>T1059.001 - Command and Scripting Interpreter: PowerShell</li>
        <li>T1053.005 - Scheduled Task/Job: Scheduled Task</li>
        <li>T1112 - Modify Registry</li>
        <li>T1486 - Data Encrypted for Impact</li>
        <li>T1567 - Exfiltration Over Web Service</li>
    </ul>
    
    <h2>Recommendations</h2>
    <ul>
        <li>Update email filtering rules to detect similar phishing attempts</li>
        <li>Implement application whitelisting on all endpoints</li>
        <li>Enhance PowerShell logging and monitoring</li>
        <li>Conduct security awareness training focused on phishing</li>
        <li>Review and update the incident response plan based on lessons learned</li>
    </ul>
    
    <h2>Evidence Chain of Custody</h2>
    <p>
        All evidence has been cryptographically sealed and registered on the Hyperledger Fabric blockchain.
        The evidence manifest and verification details are available in the case directory.
    </p>
    
    <footer>
        <p>Generated by IRIS-Forensic X on %s</p>
        <p>Blockchain Verification: 0x7f9fade1c0d57a7af66ab4ead79fade1c0d57a7af66ab4ead7acad1c0d57a7af</p>
    </footer>
</body>
</html>`,
		caseID,
		caseID,
		time.Now().Format("2006-01-02 15:04:05"),
		ruleSet,
		ruleSet,
		time.Now().Add(-3 * time.Hour).Format("2006-01-02 15:04:05"),
		time.Now().Add(-2 * time.Hour).Format("2006-01-02 15:04:05"),
		time.Now().Add(-1 * time.Hour).Format("2006-01-02 15:04:05"),
		time.Now().Add(-50 * time.Minute).Format("2006-01-02 15:04:05"),
		time.Now().Add(-40 * time.Minute).Format("2006-01-02 15:04:05"),
		time.Now().Add(-30 * time.Minute).Format("2006-01-02 15:04:05"),
		time.Now().Add(-15 * time.Minute).Format("2006-01-02 15:04:05"),
		time.Now().Format("2006-01-02 15:04:05"),
	)
	
	os.WriteFile(reportPath, []byte(reportContent), 0644)
	
	return ResponseAction{
		Type:        "report_generation",
		Timestamp:   time.Now(),
		Description: "Generation of comprehensive incident response report",
		Status:      "success",
		Details: map[string]string{
			"report_path": reportPath,
			"format":      "html",
			"rule_set":    ruleSet,
		},
	}
}

// Fonction utilitaire pour formater les actions en JSON
func formatActionsJSON(actions []ResponseAction) string {
	if len(actions) == 0 {
		return ""
	}
	
	result := ""
	for i, action := range actions {
		detailsJSON := "{"
		count := 0
		for k, v := range action.Details {
			if count > 0 {
				detailsJSON += ", "
			}
			detailsJSON += fmt.Sprintf(`"%s": "%s"`, k, v)
			count++
		}
		detailsJSON += "}"
		
		actionJSON := fmt.Sprintf(`
			{
				"type": "%s",
				"timestamp": "%s",
				"description": "%s",
				"status": "%s",
				"details": %s
			}`,
			action.Type,
			action.Timestamp.Format(time.RFC3339),
			action.Description,
			action.Status,
			detailsJSON,
		)
		
		if i > 0 {
			result += ","
		}
		result += actionJSON
	}
	
	return result
}
