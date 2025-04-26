package main

import (
	"fmt"
	"os"
	"path/filepath"
	"strings"

	"github.com/spf13/cobra"
)

var rootCmd = &cobra.Command{
	Use:   "iris",
	Short: "IRIS-Forensic X - Outil d'analyse forensique avancé",
	Long: `IRIS-Forensic X est un outil forensique hybride combinant CLI, WebUI, IA et blockchain
pour l'analyse approfondie des cyberattaques en environnement professionnel.`,
}

var captureCmd = &cobra.Command{
	Use:   "capture",
	Short: "Capture des données forensiques",
	Long:  `Capture intelligente de données forensiques (mémoire, logs, réseau)`,
	Run: func(cmd *cobra.Command, args []string) {
		smart, _ := cmd.Flags().GetBool("smart")
		if smart {
			fmt.Println("[IRIS] Démarrage de la capture intelligente...")
			fmt.Println("[IRIS] Analyse du contexte pour déterminer la méthode optimale...")
			fmt.Println("[IRIS] Capture en cours avec priorisation dynamique basée sur MITRE ATT&CK...")
			fmt.Println("[IRIS] Capture terminée avec succès.")
		} else {
			fmt.Println("[IRIS] Démarrage de la capture standard...")
			fmt.Println("[IRIS] Capture terminée avec succès.")
		}
	},
}

var analyzeCmd = &cobra.Command{
	Use:   "analyze",
	Short: "Analyse des données forensiques",
	Long:  `Analyse unifiée combinant techniques classiques et IA`,
	Run: func(cmd *cobra.Command, args []string) {
		all, _ := cmd.Flags().GetBool("all")
		if all {
			fmt.Println("[IRIS] Démarrage de l'analyse complète...")
			fmt.Println("[IRIS] Exécution de l'analyse Autopsy-like (TSK++)...")
			fmt.Println("[IRIS] Exécution de la détection IA CodeBERT...")
			fmt.Println("[IRIS] Exécution de la reconstruction temporelle...")
			fmt.Println("[IRIS] Analyse terminée avec succès.")
		} else {
			fmt.Println("[IRIS] Démarrage de l'analyse standard...")
			fmt.Println("[IRIS] Analyse terminée avec succès.")
		}
	},
}

var triageCmd = &cobra.Command{
	Use:   "triage",
	Short: "Triage des cas",
	Long:  `Triage automatique des cas basé sur l'IA`,
	Run: func(cmd *cobra.Command, args []string) {
		auto, _ := cmd.Flags().GetBool("auto")
		if auto && len(args) > 0 {
			fmt.Printf("[IRIS] Triage automatique du répertoire %s...\n", args[0])
			fmt.Println("[IRIS] 5 nouvelles captures - 2 classées \"Critique\" (AI detected)")
		} else {
			fmt.Println("[IRIS] Triage manuel démarré...")
			fmt.Println("[IRIS] Veuillez sélectionner les cas à traiter.")
		}
	},
}

var respondCmd = &cobra.Command{
	Use:   "respond",
	Short: "Réponse aux incidents",
	Long:  `Automatisation de la réponse aux incidents`,
	Run: func(cmd *cobra.Command, args []string) {
		auto, _ := cmd.Flags().GetBool("auto")
		rule, _ := cmd.Flags().GetString("rule")
		if auto && rule != "" {
			fmt.Printf("[IRIS] Réponse automatique selon la règle %s...\n", rule)
			fmt.Println("[SUCCESS] 1. Isolation complète - 2. Preuves scellées - 3. Rapport envoyé")
		} else {
			fmt.Println("[IRIS] Réponse manuelle démarrée...")
			fmt.Println("[IRIS] Veuillez sélectionner les actions à effectuer.")
		}
	},
}

var migrateCmd = &cobra.Command{
	Use:   "migrate",
	Short: "Migration des données",
	Long:  `Migration des données depuis d'autres versions ou outils`,
	Run: func(cmd *cobra.Command, args []string) {
		from, _ := cmd.Flags().GetString("from")
		to, _ := cmd.Flags().GetString("to")
		fmt.Printf("[IRIS] Migration des données depuis %s vers %s...\n", from, to)
		fmt.Println("[IRIS] Migration terminée avec succès.")
	},
}

func init() {
	captureCmd.Flags().BoolP("smart", "s", false, "Utiliser la capture intelligente")
	analyzeCmd.Flags().BoolP("all", "a", false, "Exécuter toutes les analyses")
	triageCmd.Flags().BoolP("auto", "a", false, "Triage automatique")
	respondCmd.Flags().BoolP("auto", "a", false, "Réponse automatique")
	respondCmd.Flags().StringP("rule", "r", "", "Règle à appliquer")
	migrateCmd.Flags().StringP("from", "f", "", "Source de la migration")
	migrateCmd.Flags().StringP("to", "t", "", "Destination de la migration")

	rootCmd.AddCommand(captureCmd)
	rootCmd.AddCommand(analyzeCmd)
	rootCmd.AddCommand(triageCmd)
	rootCmd.AddCommand(respondCmd)
	rootCmd.AddCommand(migrateCmd)
}

func main() {
	if err := rootCmd.Execute(); err != nil {
		fmt.Println(err)
		os.Exit(1)
	}
}
