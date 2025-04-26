import os
import numpy as np
import tensorflow as tf
from tensorflow import keras
from tensorflow.keras import layers
import onnx
import tf2onnx

class CodeBERTDetector:
    """
    Modèle de détection basé sur CodeBERT pour l'analyse de code malveillant
    et la détection de comportements suspects dans les fichiers.
    """
    
    def __init__(self, model_path=None, max_sequence_length=512):
        """
        Initialise le détecteur CodeBERT.
        
        Args:
            model_path: Chemin vers un modèle pré-entraîné (si None, crée un nouveau modèle)
            max_sequence_length: Longueur maximale des séquences d'entrée
        """
        self.max_sequence_length = max_sequence_length
        self.model = None
        
        if model_path and os.path.exists(model_path):
            self.load_model(model_path)
        else:
            self.build_model()
    
    def build_model(self):
        """
        Construit l'architecture du modèle CodeBERT pour la détection.
        """
        # Couche d'entrée pour les tokens
        input_ids = keras.Input(shape=(self.max_sequence_length,), dtype=tf.int32, name="input_ids")
        attention_mask = keras.Input(shape=(self.max_sequence_length,), dtype=tf.int32, name="attention_mask")
        
        # Couche d'embedding (simulée ici, dans un cas réel on utiliserait un modèle pré-entraîné)
        embedding_dim = 768
        embeddings = layers.Embedding(input_dim=30000, output_dim=embedding_dim)(input_ids)
        embeddings = embeddings * tf.expand_dims(tf.cast(attention_mask, tf.float32), -1)
        
        # Couches de transformation
        x = layers.Bidirectional(layers.LSTM(256, return_sequences=True))(embeddings)
        x = layers.Bidirectional(layers.LSTM(128))(x)
        
        # Couches de classification
        x = layers.Dense(128, activation="relu")(x)
        x = layers.Dropout(0.2)(x)
        x = layers.Dense(64, activation="relu")(x)
        x = layers.Dropout(0.1)(x)
        
        # Sorties multiples pour différents types de détection
        malware_output = layers.Dense(1, activation="sigmoid", name="malware_detection")(x)
        ransomware_output = layers.Dense(1, activation="sigmoid", name="ransomware_detection")(x)
        backdoor_output = layers.Dense(1, activation="sigmoid", name="backdoor_detection")(x)
        
        # Création du modèle
        self.model = keras.Model(
            inputs=[input_ids, attention_mask],
            outputs=[malware_output, ransomware_output, backdoor_output]
        )
        
        # Compilation du modèle
        self.model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=1e-5),
            loss={
                "malware_detection": "binary_crossentropy",
                "ransomware_detection": "binary_crossentropy",
                "backdoor_detection": "binary_crossentropy"
            },
            metrics={
                "malware_detection": ["accuracy", "AUC"],
                "ransomware_detection": ["accuracy", "AUC"],
                "backdoor_detection": ["accuracy", "AUC"]
            }
        )
    
    def train(self, train_data, validation_data=None, epochs=10, batch_size=32):
        """
        Entraîne le modèle sur les données fournies.
        
        Args:
            train_data: Tuple (X_train, y_train) où X_train est un tuple (input_ids, attention_mask)
                       et y_train est un tuple (malware_labels, ransomware_labels, backdoor_labels)
            validation_data: Données de validation au même format que train_data
            epochs: Nombre d'époques d'entraînement
            batch_size: Taille des batchs
            
        Returns:
            Historique d'entraînement
        """
        X_train, y_train = train_data
        
        return self.model.fit(
            X_train,
            y_train,
            validation_data=validation_data,
            epochs=epochs,
            batch_size=batch_size,
            callbacks=[
                keras.callbacks.EarlyStopping(patience=3, restore_best_weights=True),
                keras.callbacks.ReduceLROnPlateau(factor=0.2, patience=2)
            ]
        )
    
    def predict(self, input_data):
        """
        Effectue des prédictions sur les données d'entrée.
        
        Args:
            input_data: Tuple (input_ids, attention_mask)
            
        Returns:
            Tuple (malware_probs, ransomware_probs, backdoor_probs)
        """
        return self.model.predict(input_data)
    
    def save_model(self, model_path):
        """
        Sauvegarde le modèle au format Keras.
        
        Args:
            model_path: Chemin où sauvegarder le modèle
        """
        self.model.save(model_path)
    
    def load_model(self, model_path):
        """
        Charge un modèle pré-entraîné.
        
        Args:
            model_path: Chemin vers le modèle à charger
        """
        self.model = keras.models.load_model(model_path)
    
    def export_to_onnx(self, onnx_path):
        """
        Exporte le modèle au format ONNX pour une inférence optimisée.
        
        Args:
            onnx_path: Chemin où sauvegarder le modèle ONNX
        """
        # Création des entrées d'exemple pour la conversion
        input_signature = [
            tf.TensorSpec((None, self.max_sequence_length), tf.int32, name="input_ids"),
            tf.TensorSpec((None, self.max_sequence_length), tf.int32, name="attention_mask")
        ]
        
        # Conversion du modèle TensorFlow en ONNX
        onnx_model, _ = tf2onnx.convert.from_keras(self.model, input_signature, opset=13)
        
        # Sauvegarde du modèle ONNX
        onnx.save(onnx_model, onnx_path)
        
        print(f"Modèle exporté avec succès au format ONNX: {onnx_path}")

    def evaluate(self, test_data):
        """
        Évalue le modèle sur des données de test.
        
        Args:
            test_data: Tuple (X_test, y_test) au même format que pour train()
            
        Returns:
            Métriques d'évaluation
        """
        X_test, y_test = test_data
        return self.model.evaluate(X_test, y_test)


class TemporalReconstructor:
    """
    Modèle de reconstruction temporelle pour l'analyse forensique.
    Permet de reconstruire la chronologie des événements à partir des artefacts.
    """
    
    def __init__(self, model_path=None, sequence_length=100, feature_dim=128):
        """
        Initialise le reconstructeur temporel.
        
        Args:
            model_path: Chemin vers un modèle pré-entraîné (si None, crée un nouveau modèle)
            sequence_length: Longueur des séquences temporelles
            feature_dim: Dimension des caractéristiques d'entrée
        """
        self.sequence_length = sequence_length
        self.feature_dim = feature_dim
        self.model = None
        
        if model_path and os.path.exists(model_path):
            self.load_model(model_path)
        else:
            self.build_model()
    
    def build_model(self):
        """
        Construit l'architecture du modèle de reconstruction temporelle.
        """
        # Couche d'entrée pour les séquences d'événements
        inputs = keras.Input(shape=(self.sequence_length, self.feature_dim))
        
        # Encodeur
        encoder = layers.Bidirectional(layers.LSTM(256, return_sequences=True))(inputs)
        encoder = layers.Bidirectional(layers.LSTM(128, return_sequences=True))(encoder)
        
        # Mécanisme d'attention
        attention = layers.Dense(1, activation="tanh")(encoder)
        attention = layers.Flatten()(attention)
        attention = layers.Activation("softmax")(attention)
        attention = layers.RepeatVector(256)(attention)
        attention = layers.Permute([2, 1])(attention)
        
        # Application de l'attention
        context = layers.Multiply()([encoder, attention])
        context = layers.Lambda(lambda x: tf.reduce_sum(x, axis=1))(context)
        
        # Décodeur pour la reconstruction temporelle
        decoder = layers.RepeatVector(self.sequence_length)(context)
        decoder = layers.LSTM(256, return_sequences=True)(decoder)
        decoder = layers.TimeDistributed(layers.Dense(128, activation="relu"))(decoder)
        
        # Sorties
        event_type_output = layers.TimeDistributed(layers.Dense(50, activation="softmax"), name="event_type")(decoder)
        timestamp_output = layers.TimeDistributed(layers.Dense(1), name="timestamp")(decoder)
        confidence_output = layers.TimeDistributed(layers.Dense(1, activation="sigmoid"), name="confidence")(decoder)
        
        # Création du modèle
        self.model = keras.Model(
            inputs=inputs,
            outputs=[event_type_output, timestamp_output, confidence_output]
        )
        
        # Compilation du modèle
        self.model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=1e-4),
            loss={
                "event_type": "categorical_crossentropy",
                "timestamp": "mse",
                "confidence": "binary_crossentropy"
            },
            metrics={
                "event_type": ["accuracy"],
                "timestamp": ["mae"],
                "confidence": ["accuracy"]
            }
        )
    
    def train(self, train_data, validation_data=None, epochs=20, batch_size=32):
        """
        Entraîne le modèle sur les données fournies.
        
        Args:
            train_data: Tuple (X_train, y_train) où X_train est un tableau de séquences d'événements
                       et y_train est un tuple (event_types, timestamps, confidences)
            validation_data: Données de validation au même format que train_data
            epochs: Nombre d'époques d'entraînement
            batch_size: Taille des batchs
            
        Returns:
            Historique d'entraînement
        """
        X_train, y_train = train_data
        
        return self.model.fit(
            X_train,
            y_train,
            validation_data=validation_data,
            epochs=epochs,
            batch_size=batch_size,
            callbacks=[
                keras.callbacks.EarlyStopping(patience=5, restore_best_weights=True),
                keras.callbacks.ReduceLROnPlateau(factor=0.2, patience=3)
            ]
        )
    
    def reconstruct(self, event_sequences):
        """
        Reconstruit la chronologie temporelle à partir de séquences d'événements.
        
        Args:
            event_sequences: Séquences d'événements à analyser
            
        Returns:
            Tuple (event_types, timestamps, confidences)
        """
        return self.model.predict(event_sequences)
    
    def save_model(self, model_path):
        """
        Sauvegarde le modèle au format Keras.
        
        Args:
            model_path: Chemin où sauvegarder le modèle
        """
        self.model.save(model_path)
    
    def load_model(self, model_path):
        """
        Charge un modèle pré-entraîné.
        
        Args:
            model_path: Chemin vers le modèle à charger
        """
        self.model = keras.models.load_model(model_path)
    
    def export_to_onnx(self, onnx_path):
        """
        Exporte le modèle au format ONNX pour une inférence optimisée.
        
        Args:
            onnx_path: Chemin où sauvegarder le modèle ONNX
        """
        # Création des entrées d'exemple pour la conversion
        input_signature = [
            tf.TensorSpec((None, self.sequence_length, self.feature_dim), tf.float32, name="event_sequences")
        ]
        
        # Conversion du modèle TensorFlow en ONNX
        onnx_model, _ = tf2onnx.convert.from_keras(self.model, input_signature, opset=13)
        
        # Sauvegarde du modèle ONNX
        onnx.save(onnx_model, onnx_path)
        
        print(f"Modèle exporté avec succès au format ONNX: {onnx_path}")


class MalwareClassifier:
    """
    Modèle de classification de malware basé sur des caractéristiques extraites
    des fichiers et des comportements système.
    """
    
    def __init__(self, model_path=None, num_features=256, num_classes=10):
        """
        Initialise le classificateur de malware.
        
        Args:
            model_path: Chemin vers un modèle pré-entraîné (si None, crée un nouveau modèle)
            num_features: Nombre de caractéristiques d'entrée
            num_classes: Nombre de classes de malware à prédire
        """
        self.num_features = num_features
        self.num_classes = num_classes
        self.model = None
        
        if model_path and os.path.exists(model_path):
            self.load_model(model_path)
        else:
            self.build_model()
    
    def build_model(self):
        """
        Construit l'architecture du modèle de classification de malware.
        """
        # Couche d'entrée
        inputs = keras.Input(shape=(self.num_features,))
        
        # Couches cachées
        x = layers.Dense(512, activation="relu")(inputs)
        x = layers.BatchNormalization()(x)
        x = layers.Dropout(0.3)(x)
        
        x = layers.Dense(256, activation="relu")(x)
        x = layers.BatchNormalization()(x)
        x = layers.Dropout(0.3)(x)
        
        x = layers.Dense(128, activation="relu")(x)
        x = layers.BatchNormalization()(x)
        x = layers.Dropout(0.2)(x)
        
        # Couche de sortie
        outputs = layers.Dense(self.num_classes, activation="softmax")(x)
        
        # Création du modèle
        self.model = keras.Model(inputs=inputs, outputs=outputs)
        
        # Compilation du modèle
        self.model.compile(
            optimizer=keras.optimizers.Adam(learning_rate=1e-3),
            loss="categorical_crossentropy",
            metrics=["accuracy", "AUC"]
        )
    
    def train(self, train_data, validation_data=None, epochs=30, batch_size=64):
        """
        Entraîne le modèle sur les données fournies.
        
        Args:
            train_data: Tuple (X_train, y_train) où X_train est un tableau de caractéristiques
                       et y_train est un tableau de classes (one-hot encoded)
            validation_data: Données de validation au même format que train_data
            epochs: Nombre d'époques d'entraînement
            batch_size: Taille des batchs
            
        Returns:
            Historique d'entraînement
        """
        X_train, y_train = train_data
        
        return self.model.fit(
            X_train,
            y_train,
            validation_data=validation_data,
            epochs=epochs,
            batch_size=batch_size,
            callbacks=[
                keras.callbacks.EarlyStopping(patience=5, restore_best_weights=True),
                keras.callbacks.ReduceLROnPlateau(factor=0.2, patience=3)
            ]
        )
    
    def predict(self, features):
        """
        Classifie les échantillons en fonction de leurs caractéristiques.
        
        Args:
            features: Caractéristiques extraites des échantillons
            
        Returns:
            Probabilités d'appartenance à chaque classe
        """
        return self.model.predict(features)
    
    def save_model(self, model_path):
        """
        Sauvegarde le modèle au format Keras.
        
        Args:
            model_path: Chemin où sauvegarder le modèle
        """
        self.model.save(model_path)
    
    def load_model(self, model_path):
        """
        Charge un modèle pré-entraîné.
        
        Args:
            model_path: Chemin vers le modèle à charger
        """
        self.model = keras.models.load_model(model_path)
    
    def export_to_onnx(self, onnx_path):
        """
        Exporte le modèle au format ONNX pour une inférence optimisée.
        
        Args:
            onnx_path: Chemin où sauvegarder le modèle ONNX
        """
        # Création des entrées d'exemple pour la conversion
        input_signature = [
            tf.TensorSpec((None, self.num_features), tf.float32, name="features")
        ]
        
        # Conversion du modèle TensorFlow en ONNX
        onnx_model, _ = tf2onnx.convert.from_keras(self.model, input_signature, opset=13)
        
        # Sauvegarde du modèle ONNX
        onnx.save(onnx_model, onnx_path)
        
        print(f"Modèle exporté avec succès au format ONNX: {onnx_path}")


# Classe de gestion des modèles IA
class AIManager:
    """
    Gestionnaire central pour les modèles d'IA d'IRIS-Forensic X.
    Permet de charger, entraîner et utiliser les différents modèles.
    """
    
    def __init__(self, models_dir="./models"):
        """
        Initialise le gestionnaire de modèles IA.
        
        Args:
            models_dir: Répertoire où stocker les modèles
        """
        self.models_dir = models_dir
        os.makedirs(models_dir, exist_ok=True)
        
        # Dictionnaire des modèles disponibles
        self.models = {
            "codebert_detector": None,
            "temporal_reconstructor": None,
            "malware_classifier": None
        }
        
        # Tenter de charger les modèles existants
        self._load_available_models()
    
    def _load_available_models(self):
        """
        Charge les modèles disponibles dans le répertoire des modèles.
        """
        # CodeBERT Detector
        codebert_path = os.path.join(self.models_dir, "codebert_detector")
        if os.path.exists(codebert_path):
            try:
                self.models["codebert_detector"] = CodeBERTDetector(model_path=codebert_path)
                print(f"Modèle CodeBERT Detector chargé depuis {codebert_path}")
            except Exception as e:
                print(f"Erreur lors du chargement du modèle CodeBERT Detector: {e}")
        
        # Temporal Reconstructor
        temporal_path = os.path.join(self.models_dir, "temporal_reconstructor")
        if os.path.exists(temporal_path):
            try:
                self.models["temporal_reconstructor"] = TemporalReconstructor(model_path=temporal_path)
                print(f"Modèle Temporal Reconstructor chargé depuis {temporal_path}")
            except Exception as e:
                print(f"Erreur lors du chargement du modèle Temporal Reconstructor: {e}")
        
        # Malware Classifier
        malware_path = os.path.join(self.models_dir, "malware_classifier")
        if os.path.exists(malware_path):
            try:
                self.models["malware_classifier"] = MalwareClassifier(model_path=malware_path)
                print(f"Modèle Malware Classifier chargé depuis {malware_path}")
            except Exception as e:
                print(f"Erreur lors du chargement du modèle Malware Classifier: {e}")
    
    def get_model(self, model_name):
        """
        Récupère un modèle par son nom.
        
        Args:
            model_name: Nom du modèle à récupérer
            
        Returns:
            Instance du modèle demandé ou None si non disponible
        """
        if model_name not in self.models:
            print(f"Modèle {model_name} non reconnu")
            return None
        
        if self.models[model_name] is None:
            # Initialiser le modèle s'il n'est pas encore chargé
            if model_name == "codebert_detector":
                self.models[model_name] = CodeBERTDetector()
            elif model_name == "temporal_reconstructor":
                self.models[model_name] = TemporalReconstructor()
            elif model_name == "malware_classifier":
                self.models[model_name] = MalwareClassifier()
        
        return self.models[model_name]
    
    def train_model(self, model_name, train_data, validation_data=None, **kwargs):
        """
        Entraîne un modèle spécifique.
        
        Args:
            model_name: Nom du modèle à entraîner
            train_data: Données d'entraînement
            validation_data: Données de validation
            **kwargs: Arguments supplémentaires pour l'entraînement
            
        Returns:
            Historique d'entraînement ou None en cas d'erreur
        """
        model = self.get_model(model_name)
        if model is None:
            return None
        
        try:
            history = model.train(train_data, validation_data, **kwargs)
            
            # Sauvegarder le modèle après l'entraînement
            model_path = os.path.join(self.models_dir, model_name)
            model.save_model(model_path)
            
            # Exporter en ONNX pour l'inférence optimisée
            onnx_path = os.path.join(self.models_dir, f"{model_name}.onnx")
            model.export_to_onnx(onnx_path)
            
            return history
        except Exception as e:
            print(f"Erreur lors de l'entraînement du modèle {model_name}: {e}")
            return None
    
    def analyze_file(self, file_path):
        """
        Analyse un fichier avec tous les modèles disponibles.
        
        Args:
            file_path: Chemin vers le fichier à analyser
            
        Returns:
            Dictionnaire des résultats d'analyse
        """
        results = {
            "file_path": file_path,
            "analysis_timestamp": tf.timestamp().numpy(),
            "detections": {},
            "temporal_analysis": None,
            "classification": None
        }
        
        # Simuler l'extraction de caractéristiques (dans une implémentation réelle,
        # cette partie serait beaucoup plus élaborée)
        try:
            # Pour CodeBERT Detector
            if self.models["codebert_detector"]:
                # Simuler des entrées pour le modèle
                input_ids = np.random.randint(0, 30000, (1, 512))
                attention_mask = np.ones((1, 512))
                
                malware_prob, ransomware_prob, backdoor_prob = self.models["codebert_detector"].predict(
                    [input_ids, attention_mask]
                )
                
                results["detections"] = {
                    "malware": float(malware_prob[0][0]),
                    "ransomware": float(ransomware_prob[0][0]),
                    "backdoor": float(backdoor_prob[0][0])
                }
            
            # Pour Temporal Reconstructor
            if self.models["temporal_reconstructor"]:
                # Simuler des séquences d'événements
                event_sequences = np.random.random((1, 100, 128))
                
                event_types, timestamps, confidences = self.models["temporal_reconstructor"].reconstruct(
                    event_sequences
                )
                
                results["temporal_analysis"] = {
                    "event_count": len(timestamps[0]),
                    "first_event_time": float(timestamps[0][0][0]),
                    "last_event_time": float(timestamps[0][-1][0]),
                    "average_confidence": float(np.mean(confidences))
                }
            
            # Pour Malware Classifier
            if self.models["malware_classifier"]:
                # Simuler des caractéristiques
                features = np.random.random((1, 256))
                
                class_probs = self.models["malware_classifier"].predict(features)
                
                top_class_idx = np.argmax(class_probs[0])
                top_class_prob = float(class_probs[0][top_class_idx])
                
                # Simuler des noms de classes
                class_names = [
                    "ransomware", "trojan", "worm", "spyware", "adware",
                    "backdoor", "rootkit", "botnet", "keylogger", "cryptominer"
                ]
                
                results["classification"] = {
                    "class": class_names[top_class_idx],
                    "confidence": top_class_prob,
                    "all_classes": {
                        class_names[i]: float(class_probs[0][i])
                        for i in range(len(class_names))
                    }
                }
            
            # Calculer un score global de menace
            threat_score = 0.0
            count = 0
            
            if "detections" in results:
                threat_score += sum(results["detections"].values())
                count += len(results["detections"])
            
            if "classification" in results and results["classification"]:
                threat_score += results["classification"]["confidence"]
                count += 1
            
            if count > 0:
                results["threat_score"] = threat_score / count
            else:
                results["threat_score"] = 0.0
            
            return results
        
        except Exception as e:
            print(f"Erreur lors de l'analyse du fichier {file_path}: {e}")
            return {
                "file_path": file_path,
                "error": str(e),
                "threat_score": 0.0
            }
