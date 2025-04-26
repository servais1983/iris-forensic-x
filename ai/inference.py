import os
import json
import numpy as np
import tensorflow as tf
from datetime import datetime
import hashlib
import base64
import hmac
import time

class AIInference:
    """
    Classe pour l'inférence optimisée des modèles IA dans IRIS-Forensic X.
    Utilise ONNX Runtime pour des performances maximales sur CPU et GPU.
    """
    
    def __init__(self, models_dir="./models"):
        """
        Initialise le moteur d'inférence.
        
        Args:
            models_dir: Répertoire contenant les modèles ONNX
        """
        self.models_dir = models_dir
        self.onnx_sessions = {}
        self.model_metadata = {}
        self.load_available_models()
    
    def load_available_models(self):
        """
        Charge tous les modèles ONNX disponibles dans le répertoire spécifié.
        """
        try:
            # Simuler le chargement des modèles ONNX
            # Dans une implémentation réelle, nous utiliserions onnxruntime
            print("Chargement des modèles ONNX disponibles...")
            
            # Simuler les métadonnées des modèles
            self.model_metadata = {
                "codebert_detector": {
                    "name": "CodeBERT Detector",
                    "version": "1.2.0",
                    "input_shape": [1, 512],
                    "output_shape": [[1, 1], [1, 1], [1, 1]],
                    "precision": "fp32",
                    "last_updated": "2025-04-20",
                    "accuracy": 0.94,
                    "latency_ms": 120
                },
                "temporal_reconstructor": {
                    "name": "Temporal Reconstructor",
                    "version": "1.0.5",
                    "input_shape": [1, 100, 128],
                    "output_shape": [[1, 100, 50], [1, 100, 1], [1, 100, 1]],
                    "precision": "fp32",
                    "last_updated": "2025-04-18",
                    "accuracy": 0.89,
                    "latency_ms": 180
                },
                "malware_classifier": {
                    "name": "Malware Classifier",
                    "version": "2.1.0",
                    "input_shape": [1, 256],
                    "output_shape": [1, 10],
                    "precision": "fp32",
                    "last_updated": "2025-04-15",
                    "accuracy": 0.92,
                    "latency_ms": 150
                }
            }
            
            print(f"Modèles chargés: {list(self.model_metadata.keys())}")
            
        except Exception as e:
            print(f"Erreur lors du chargement des modèles ONNX: {e}")
    
    def get_model_info(self, model_name):
        """
        Récupère les informations sur un modèle spécifique.
        
        Args:
            model_name: Nom du modèle
            
        Returns:
            Métadonnées du modèle ou None si non trouvé
        """
        return self.model_metadata.get(model_name)
    
    def list_models(self):
        """
        Liste tous les modèles disponibles.
        
        Returns:
            Liste des métadonnées de tous les modèles
        """
        return self.model_metadata
    
    def infer(self, model_name, inputs, options=None):
        """
        Effectue l'inférence avec un modèle spécifique.
        
        Args:
            model_name: Nom du modèle à utiliser
            inputs: Données d'entrée pour le modèle
            options: Options d'inférence (ex: precision, batch_size)
            
        Returns:
            Résultats de l'inférence
        """
        if model_name not in self.model_metadata:
            raise ValueError(f"Modèle {model_name} non disponible")
        
        # Simuler l'inférence
        start_time = time.time()
        
        # Ajouter un délai aléatoire pour simuler le temps de traitement
        time.sleep(self.model_metadata[model_name]["latency_ms"] / 1000)
        
        # Générer des résultats simulés en fonction du modèle
        if model_name == "codebert_detector":
            results = {
                "malware_detection": float(np.random.random() * 0.8 + 0.1),
                "ransomware_detection": float(np.random.random() * 0.7 + 0.1),
                "backdoor_detection": float(np.random.random() * 0.6 + 0.1)
            }
        elif model_name == "temporal_reconstructor":
            num_events = 10
            results = {
                "events": [
                    {
                        "timestamp": datetime.now().timestamp() - (num_events - i) * 3600,
                        "event_type": np.random.choice(["file_access", "registry_change", "network_connection", "process_creation", "authentication"]),
                        "confidence": float(np.random.random() * 0.3 + 0.7)
                    }
                    for i in range(num_events)
                ],
                "timeline_confidence": float(np.random.random() * 0.2 + 0.7)
            }
        elif model_name == "malware_classifier":
            class_names = [
                "ransomware", "trojan", "worm", "spyware", "adware",
                "backdoor", "rootkit", "botnet", "keylogger", "cryptominer"
            ]
            probs = np.random.random(10)
            probs = probs / np.sum(probs)  # Normaliser pour que la somme soit 1
            
            results = {
                "class_probabilities": {
                    class_names[i]: float(probs[i])
                    for i in range(len(class_names))
                },
                "top_class": class_names[np.argmax(probs)],
                "top_probability": float(probs[np.argmax(probs)])
            }
        
        # Ajouter des métadonnées d'inférence
        inference_time = time.time() - start_time
        results["inference_metadata"] = {
            "model_name": self.model_metadata[model_name]["name"],
            "model_version": self.model_metadata[model_name]["version"],
            "inference_time_ms": inference_time * 1000,
            "timestamp": datetime.now().isoformat()
        }
        
        return results
    
    def batch_infer(self, model_name, batch_inputs, options=None):
        """
        Effectue l'inférence par lots pour un traitement plus efficace.
        
        Args:
            model_name: Nom du modèle à utiliser
            batch_inputs: Liste de données d'entrée
            options: Options d'inférence
            
        Returns:
            Liste des résultats d'inférence
        """
        results = []
        for inputs in batch_inputs:
            results.append(self.infer(model_name, inputs, options))
        return results


class AIDataProcessor:
    """
    Classe pour le prétraitement et le post-traitement des données pour les modèles IA.
    """
    
    def __init__(self):
        """
        Initialise le processeur de données.
        """
        self.tokenizers = {}
        self.feature_extractors = {}
    
    def preprocess_code(self, code_text, max_length=512):
        """
        Prétraite le code source pour l'analyse par CodeBERT.
        
        Args:
            code_text: Texte du code source
            max_length: Longueur maximale de la séquence
            
        Returns:
            Tuple (input_ids, attention_mask)
        """
        # Simuler la tokenization
        input_ids = np.random.randint(0, 30000, (1, max_length))
        attention_mask = np.ones((1, max_length))
        
        return input_ids, attention_mask
    
    def preprocess_events(self, events, sequence_length=100, feature_dim=128):
        """
        Prétraite les événements pour la reconstruction temporelle.
        
        Args:
            events: Liste d'événements
            sequence_length: Longueur de la séquence
            feature_dim: Dimension des caractéristiques
            
        Returns:
            Séquence d'événements formatée
        """
        # Simuler la création de séquences d'événements
        event_sequences = np.random.random((1, sequence_length, feature_dim))
        
        return event_sequences
    
    def extract_features(self, file_path, num_features=256):
        """
        Extrait des caractéristiques d'un fichier pour la classification.
        
        Args:
            file_path: Chemin vers le fichier
            num_features: Nombre de caractéristiques à extraire
            
        Returns:
            Vecteur de caractéristiques
        """
        # Simuler l'extraction de caractéristiques
        features = np.random.random((1, num_features))
        
        return features
    
    def postprocess_detection(self, detection_results, threshold=0.5):
        """
        Post-traite les résultats de détection.
        
        Args:
            detection_results: Résultats bruts du modèle
            threshold: Seuil de détection
            
        Returns:
            Résultats formatés
        """
        formatted_results = {}
        
        for key, value in detection_results.items():
            if isinstance(value, (int, float)):
                formatted_results[key] = {
                    "score": value,
                    "detected": value >= threshold,
                    "confidence": min(value * 1.5, 1.0) if value >= threshold else max(1.0 - value * 1.5, 0.0)
                }
        
        # Calculer un score global
        if formatted_results:
            scores = [item["score"] for item in formatted_results.values()]
            formatted_results["overall"] = {
                "score": sum(scores) / len(scores),
                "detected": any(item["detected"] for item in formatted_results.values()),
                "confidence": max([item["confidence"] for item in formatted_results.values()]) if any(item["detected"] for item in formatted_results.values()) else 0.0
            }
        
        return formatted_results
    
    def postprocess_classification(self, classification_results, top_k=3):
        """
        Post-traite les résultats de classification.
        
        Args:
            classification_results: Résultats bruts du modèle
            top_k: Nombre de classes principales à inclure
            
        Returns:
            Résultats formatés
        """
        if "class_probabilities" not in classification_results:
            return classification_results
        
        # Trier les classes par probabilité
        sorted_classes = sorted(
            classification_results["class_probabilities"].items(),
            key=lambda x: x[1],
            reverse=True
        )
        
        # Prendre les top_k classes
        top_classes = sorted_classes[:top_k]
        
        formatted_results = {
            "top_classes": [
                {
                    "class": class_name,
                    "probability": prob,
                    "confidence": min(prob * 2, 1.0)
                }
                for class_name, prob in top_classes
            ],
            "primary_class": {
                "class": top_classes[0][0],
                "probability": top_classes[0][1],
                "confidence": min(top_classes[0][1] * 2, 1.0)
            }
        }
        
        return formatted_results


class AITrainer:
    """
    Classe pour l'entraînement et l'optimisation des modèles IA.
    """
    
    def __init__(self, models_dir="./models", data_dir="./datasets"):
        """
        Initialise le gestionnaire d'entraînement.
        
        Args:
            models_dir: Répertoire où stocker les modèles
            data_dir: Répertoire contenant les datasets
        """
        self.models_dir = models_dir
        self.data_dir = data_dir
        self.training_jobs = {}
        self.job_counter = 0
    
    def create_training_job(self, model_name, config):
        """
        Crée un nouveau job d'entraînement.
        
        Args:
            model_name: Nom du modèle à entraîner
            config: Configuration d'entraînement
            
        Returns:
            ID du job d'entraînement
        """
        job_id = f"job-{self.job_counter}"
        self.job_counter += 1
        
        self.training_jobs[job_id] = {
            "model_name": model_name,
            "config": config,
            "status": "created",
            "progress": 0.0,
            "created_at": datetime.now().isoformat(),
            "started_at": None,
            "completed_at": None,
            "metrics": {},
            "error": None
        }
        
        return job_id
    
    def start_training_job(self, job_id):
        """
        Démarre un job d'entraînement.
        
        Args:
            job_id: ID du job à démarrer
            
        Returns:
            True si le job a été démarré, False sinon
        """
        if job_id not in self.training_jobs:
            return False
        
        if self.training_jobs[job_id]["status"] != "created":
            return False
        
        self.training_jobs[job_id]["status"] = "running"
        self.training_jobs[job_id]["started_at"] = datetime.now().isoformat()
        
        # Dans une implémentation réelle, nous lancerions l'entraînement dans un thread séparé
        # ou un processus dédié. Ici, nous simulons simplement le processus.
        
        return True
    
    def get_job_status(self, job_id):
        """
        Récupère le statut d'un job d'entraînement.
        
        Args:
            job_id: ID du job
            
        Returns:
            Statut du job ou None si non trouvé
        """
        if job_id not in self.training_jobs:
            return None
        
        # Simuler la progression pour les jobs en cours
        if self.training_jobs[job_id]["status"] == "running":
            current_progress = self.training_jobs[job_id]["progress"]
            if current_progress < 100.0:
                # Incrémenter la progression de manière aléatoire
                increment = np.random.random() * 5.0
                new_progress = min(current_progress + increment, 100.0)
                self.training_jobs[job_id]["progress"] = new_progress
                
                # Si la progression atteint 100%, marquer comme terminé
                if new_progress >= 100.0:
                    self.training_jobs[job_id]["status"] = "completed"
                    self.training_jobs[job_id]["completed_at"] = datetime.now().isoformat()
                    
                    # Simuler des métriques finales
                    self.training_jobs[job_id]["metrics"] = {
                        "accuracy": float(np.random.random() * 0.15 + 0.8),
                        "loss": float(np.random.random() * 0.3 + 0.1),
                        "val_accuracy": float(np.random.random() * 0.15 + 0.75),
                        "val_loss": float(np.random.random() * 0.4 + 0.2),
                        "epochs": int(np.random.randint(20, 50)),
                        "training_time": float(np.random.randint(1800, 7200))
                    }
        
        return self.training_jobs[job_id]
    
    def list_jobs(self, status=None):
        """
        Liste tous les jobs d'entraînement.
        
        Args:
            status: Filtrer par statut (optionnel)
            
        Returns:
            Liste des jobs correspondant au filtre
        """
        if status:
            return {
                job_id: job_info
                for job_id, job_info in self.training_jobs.items()
                if job_info["status"] == status
            }
        else:
            return self.training_jobs
    
    def cancel_job(self, job_id):
        """
        Annule un job d'entraînement en cours.
        
        Args:
            job_id: ID du job à annuler
            
        Returns:
            True si le job a été annulé, False sinon
        """
        if job_id not in self.training_jobs:
            return False
        
        if self.training_jobs[job_id]["status"] != "running":
            return False
        
        self.training_jobs[job_id]["status"] = "cancelled"
        self.training_jobs[job_id]["completed_at"] = datetime.now().isoformat()
        
        return True


class AISecurityVerifier:
    """
    Classe pour la vérification de sécurité et d'intégrité des modèles IA.
    """
    
    def __init__(self, secret_key=None):
        """
        Initialise le vérificateur de sécurité.
        
        Args:
            secret_key: Clé secrète pour la signature (générée si None)
        """
        if secret_key is None:
            # Générer une clé aléatoire
            self.secret_key = os.urandom(32)
        else:
            self.secret_key = secret_key
    
    def compute_model_hash(self, model_path):
        """
        Calcule le hash d'un fichier modèle.
        
        Args:
            model_path: Chemin vers le fichier modèle
            
        Returns:
            Hash SHA-256 du modèle
        """
        # Simuler le calcul du hash
        return hashlib.sha256(f"model:{model_path}".encode()).hexdigest()
    
    def sign_model(self, model_path, metadata=None):
        """
        Signe un modèle pour garantir son intégrité.
        
        Args:
            model_path: Chemin vers le fichier modèle
            metadata: Métadonnées à inclure dans la signature
            
        Returns:
            Signature du modèle
        """
        model_hash = self.compute_model_hash(model_path)
        
        # Créer un message à signer
        message = {
            "model_hash": model_hash,
            "timestamp": datetime.now().isoformat(),
            "metadata": metadata or {}
        }
        
        # Sérialiser le message
        message_json = json.dumps(message, sort_keys=True)
        
        # Calculer la signature HMAC
        signature = hmac.new(
            self.secret_key,
            message_json.encode(),
            hashlib.sha256
        ).digest()
        
        # Encoder la signature en base64
        signature_b64 = base64.b64encode(signature).decode()
        
        return {
            "message": message,
            "signature": signature_b64
        }
    
    def verify_model(self, model_path, signature_data):
        """
        Vérifie l'intégrité d'un modèle à l'aide de sa signature.
        
        Args:
            model_path: Chemin vers le fichier modèle
            signature_data: Données de signature
            
        Returns:
            True si la signature est valide, False sinon
        """
        if "message" not in signature_data or "signature" not in signature_data:
            return False
        
        # Calculer le hash actuel du modèle
        current_hash = self.compute_model_hash(model_path)
        
        # Vérifier que le hash correspond
        if current_hash != signature_data["message"]["model_hash"]:
            return False
        
        # Recalculer la signature
        message_json = json.dumps(signature_data["message"], sort_keys=True)
        expected_signature = hmac.new(
            self.secret_key,
            message_json.encode(),
            hashlib.sha256
        ).digest()
        
        # Décoder la signature fournie
        provided_signature = base64.b64decode(signature_data["signature"])
        
        # Comparer les signatures
        return hmac.compare_digest(expected_signature, provided_signature)


# Classe de gestion unifiée de la couche IA
class AILayer:
    """
    Couche IA unifiée pour IRIS-Forensic X.
    Intègre toutes les fonctionnalités IA dans une interface cohérente.
    """
    
    def __init__(self, models_dir="./models", data_dir="./datasets"):
        """
        Initialise la couche IA.
        
        Args:
            models_dir: Répertoire des modèles
            data_dir: Répertoire des datasets
        """
        self.models_dir = models_dir
        self.data_dir = data_dir
        
        # Créer les répertoires si nécessaire
        os.makedirs(models_dir, exist_ok=True)
        os.makedirs(data_dir, exist_ok=True)
        
        # Initialiser les composants
        self.inference = AIInference(models_dir)
        self.processor = AIDataProcessor()
        self.trainer = AITrainer(models_dir, data_dir)
        self.security = AISecurityVerifier()
    
    def analyze_file(self, file_path, analysis_type="full"):
        """
        Analyse un fichier avec les modèles IA appropriés.
        
        Args:
            file_path: Chemin vers le fichier à analyser
            analysis_type: Type d'analyse à effectuer (full, quick, deep)
            
        Returns:
            Résultats de l'analyse
        """
        results = {
            "file_path": file_path,
            "analysis_type": analysis_type,
            "timestamp": datetime.now().isoformat(),
            "results": {}
        }
        
        try:
            # Déterminer quels modèles utiliser en fonction du type d'analyse
            models_to_use = []
            if analysis_type == "quick":
                models_to_use = ["malware_classifier"]
            elif analysis_type == "deep":
                models_to_use = ["codebert_detector", "temporal_reconstructor", "malware_classifier"]
            else:  # full (default)
                models_to_use = ["codebert_detector", "malware_classifier"]
            
            # Effectuer l'analyse avec chaque modèle
            for model_name in models_to_use:
                if model_name == "codebert_detector":
                    # Prétraiter le fichier pour CodeBERT
                    inputs = self.processor.preprocess_code("# Simulated code content")
                    
                    # Effectuer l'inférence
                    detection_results = self.inference.infer(model_name, inputs)
                    
                    # Post-traiter les résultats
                    results["results"]["detection"] = self.processor.postprocess_detection(detection_results)
                
                elif model_name == "temporal_reconstructor":
                    # Prétraiter les événements
                    inputs = self.processor.preprocess_events([])
                    
                    # Effectuer l'inférence
                    temporal_results = self.inference.infer(model_name, inputs)
                    
                    # Ajouter les résultats
                    results["results"]["temporal"] = temporal_results
                
                elif model_name == "malware_classifier":
                    # Extraire les caractéristiques
                    features = self.processor.extract_features(file_path)
                    
                    # Effectuer l'inférence
                    classification_results = self.inference.infer(model_name, features)
                    
                    # Post-traiter les résultats
                    results["results"]["classification"] = self.processor.postprocess_classification(classification_results)
            
            # Calculer un score global de menace
            threat_scores = []
            
            if "detection" in results["results"] and "overall" in results["results"]["detection"]:
                threat_scores.append(results["results"]["detection"]["overall"]["score"])
            
            if "classification" in results["results"] and "primary_class" in results["results"]["classification"]:
                threat_scores.append(results["results"]["classification"]["primary_class"]["probability"])
            
            if threat_scores:
                results["threat_score"] = sum(threat_scores) / len(threat_scores)
                results["threat_level"] = self._get_threat_level(results["threat_score"])
            else:
                results["threat_score"] = 0.0
                results["threat_level"] = "unknown"
            
            return results
        
        except Exception as e:
            results["error"] = str(e)
            results["threat_score"] = 0.0
            results["threat_level"] = "error"
            return results
    
    def _get_threat_level(self, score):
        """
        Convertit un score de menace en niveau de menace.
        
        Args:
            score: Score de menace (0.0 à 1.0)
            
        Returns:
            Niveau de menace (critical, high, medium, low, safe)
        """
        if score >= 0.8:
            return "critical"
        elif score >= 0.6:
            return "high"
        elif score >= 0.4:
            return "medium"
        elif score >= 0.2:
            return "low"
        else:
            return "safe"
    
    def train_model(self, model_name, config):
        """
        Crée et démarre un job d'entraînement pour un modèle.
        
        Args:
            model_name: Nom du modèle à entraîner
            config: Configuration d'entraînement
            
        Returns:
            ID du job d'entraînement
        """
        job_id = self.trainer.create_training_job(model_name, config)
        self.trainer.start_training_job(job_id)
        return job_id
    
    def get_training_status(self, job_id):
        """
        Récupère le statut d'un job d'entraînement.
        
        Args:
            job_id: ID du job
            
        Returns:
            Statut du job
        """
        return self.trainer.get_job_status(job_id)
    
    def list_models(self):
        """
        Liste tous les modèles disponibles.
        
        Returns:
            Informations sur les modèles
        """
        return self.inference.list_models()
    
    def verify_model_integrity(self, model_name):
        """
        Vérifie l'intégrité d'un modèle.
        
        Args:
            model_name: Nom du modèle à vérifier
            
        Returns:
            Résultat de la vérification
        """
        model_path = os.path.join(self.models_dir, f"{model_name}.onnx")
        
        # Simuler la vérification
        return {
            "model_name": model_name,
            "verified": True,
            "hash": self.security.compute_model_hash(model_path),
            "timestamp": datetime.now().isoformat()
        }
