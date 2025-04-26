import os
import json
import time
import hashlib
import base64
from datetime import datetime
from cryptography.hazmat.primitives import hashes
from cryptography.hazmat.primitives.asymmetric import rsa, padding
from cryptography.hazmat.primitives.serialization import (
    load_pem_private_key,
    load_pem_public_key,
    Encoding,
    PrivateFormat,
    PublicFormat,
    NoEncryption
)

class BlockchainProof:
    """
    Classe pour la gestion des preuves légales basées sur la blockchain.
    Utilise Hyperledger Ursa pour la cryptographie et garantit l'intégrité des preuves.
    """
    
    def __init__(self, storage_dir="./blockchain"):
        """
        Initialise le gestionnaire de preuves blockchain.
        
        Args:
            storage_dir: Répertoire de stockage des preuves
        """
        self.storage_dir = storage_dir
        os.makedirs(storage_dir, exist_ok=True)
        
        # Chemin pour les clés
        self.keys_dir = os.path.join(storage_dir, "keys")
        os.makedirs(self.keys_dir, exist_ok=True)
        
        # Chemin pour les preuves
        self.proofs_dir = os.path.join(storage_dir, "proofs")
        os.makedirs(self.proofs_dir, exist_ok=True)
        
        # Générer ou charger les clés
        self.private_key_path = os.path.join(self.keys_dir, "private_key.pem")
        self.public_key_path = os.path.join(self.keys_dir, "public_key.pem")
        
        if not os.path.exists(self.private_key_path) or not os.path.exists(self.public_key_path):
            self._generate_keys()
        
        self.private_key = self._load_private_key()
        self.public_key = self._load_public_key()
        
        # Chaîne de blocs simulée
        self.chain_file = os.path.join(storage_dir, "chain.json")
        self.chain = self._load_chain()
    
    def _generate_keys(self):
        """
        Génère une paire de clés RSA pour la signature des preuves.
        """
        # Générer une clé privée RSA
        private_key = rsa.generate_private_key(
            public_exponent=65537,
            key_size=2048
        )
        
        # Extraire la clé publique
        public_key = private_key.public_key()
        
        # Sérialiser et sauvegarder la clé privée
        private_pem = private_key.private_bytes(
            encoding=Encoding.PEM,
            format=PrivateFormat.PKCS8,
            encryption_algorithm=NoEncryption()
        )
        with open(self.private_key_path, 'wb') as f:
            f.write(private_pem)
        
        # Sérialiser et sauvegarder la clé publique
        public_pem = public_key.public_bytes(
            encoding=Encoding.PEM,
            format=PublicFormat.SubjectPublicKeyInfo
        )
        with open(self.public_key_path, 'wb') as f:
            f.write(public_pem)
        
        print("Paire de clés générée avec succès.")
    
    def _load_private_key(self):
        """
        Charge la clé privée depuis le fichier.
        
        Returns:
            Clé privée RSA
        """
        with open(self.private_key_path, 'rb') as f:
            private_pem = f.read()
        
        return load_pem_private_key(private_pem, password=None)
    
    def _load_public_key(self):
        """
        Charge la clé publique depuis le fichier.
        
        Returns:
            Clé publique RSA
        """
        with open(self.public_key_path, 'rb') as f:
            public_pem = f.read()
        
        return load_pem_public_key(public_pem)
    
    def _load_chain(self):
        """
        Charge la chaîne de blocs depuis le fichier.
        
        Returns:
            Chaîne de blocs
        """
        if os.path.exists(self.chain_file):
            with open(self.chain_file, 'r') as f:
                return json.load(f)
        else:
            # Créer une chaîne avec un bloc genesis
            genesis_block = {
                "index": 0,
                "timestamp": datetime.now().isoformat(),
                "proof": 1,
                "previous_hash": "0",
                "transactions": []
            }
            
            chain = {
                "blocks": [genesis_block],
                "last_updated": datetime.now().isoformat()
            }
            
            with open(self.chain_file, 'w') as f:
                json.dump(chain, f, indent=2)
            
            return chain
    
    def _save_chain(self):
        """
        Sauvegarde la chaîne de blocs dans le fichier.
        """
        self.chain["last_updated"] = datetime.now().isoformat()
        
        with open(self.chain_file, 'w') as f:
            json.dump(self.chain, f, indent=2)
    
    def _hash_block(self, block):
        """
        Calcule le hash d'un bloc.
        
        Args:
            block: Bloc à hasher
            
        Returns:
            Hash du bloc
        """
        # Convertir le bloc en chaîne JSON triée
        block_string = json.dumps(block, sort_keys=True).encode()
        
        # Calculer le hash SHA-256
        return hashlib.sha256(block_string).hexdigest()
    
    def _create_proof_of_work(self, previous_proof):
        """
        Crée une preuve de travail simple.
        
        Args:
            previous_proof: Preuve du bloc précédent
            
        Returns:
            Nouvelle preuve
        """
        # Simuler un algorithme de preuve de travail simple
        proof = 1
        check_proof = False
        
        while not check_proof:
            hash_operation = hashlib.sha256(f'{previous_proof**2 - proof**2}'.encode()).hexdigest()
            if hash_operation[:4] == '0000':
                check_proof = True
            else:
                proof += 1
        
        return proof
    
    def create_proof(self, data, metadata=None):
        """
        Crée une preuve légale pour des données.
        
        Args:
            data: Données à prouver (chaîne ou dictionnaire)
            metadata: Métadonnées supplémentaires
            
        Returns:
            ID de la preuve
        """
        # Convertir les données en chaîne JSON si nécessaire
        if isinstance(data, dict):
            data_str = json.dumps(data, sort_keys=True)
        else:
            data_str = str(data)
        
        # Calculer le hash des données
        data_hash = hashlib.sha256(data_str.encode()).hexdigest()
        
        # Créer un timestamp
        timestamp = datetime.now().isoformat()
        
        # Créer la transaction
        transaction = {
            "data_hash": data_hash,
            "timestamp": timestamp,
            "metadata": metadata or {}
        }
        
        # Signer la transaction
        transaction_str = json.dumps(transaction, sort_keys=True).encode()
        signature = self.private_key.sign(
            transaction_str,
            padding.PSS(
                mgf=padding.MGF1(hashes.SHA256()),
                salt_length=padding.PSS.MAX_LENGTH
            ),
            hashes.SHA256()
        )
        
        # Encoder la signature en base64
        signature_b64 = base64.b64encode(signature).decode()
        
        # Ajouter la signature à la transaction
        transaction["signature"] = signature_b64
        
        # Générer un ID unique pour la preuve
        proof_id = hashlib.sha256(f"{data_hash}:{timestamp}".encode()).hexdigest()
        transaction["id"] = proof_id
        
        # Ajouter la transaction au dernier bloc
        last_block = self.chain["blocks"][-1]
        last_block["transactions"].append(transaction)
        
        # Si le bloc a suffisamment de transactions, en créer un nouveau
        if len(last_block["transactions"]) >= 5:
            self._mine_block()
        
        # Sauvegarder la chaîne
        self._save_chain()
        
        # Sauvegarder la preuve dans un fichier séparé
        proof_file = os.path.join(self.proofs_dir, f"{proof_id}.json")
        with open(proof_file, 'w') as f:
            json.dump(transaction, f, indent=2)
        
        return proof_id
    
    def _mine_block(self):
        """
        Mine un nouveau bloc pour la chaîne.
        
        Returns:
            Nouveau bloc
        """
        last_block = self.chain["blocks"][-1]
        previous_proof = last_block["proof"]
        
        # Calculer la nouvelle preuve
        proof = self._create_proof_of_work(previous_proof)
        
        # Calculer le hash du bloc précédent
        previous_hash = self._hash_block(last_block)
        
        # Créer le nouveau bloc
        block = {
            "index": last_block["index"] + 1,
            "timestamp": datetime.now().isoformat(),
            "proof": proof,
            "previous_hash": previous_hash,
            "transactions": []
        }
        
        # Ajouter le bloc à la chaîne
        self.chain["blocks"].append(block)
        
        return block
    
    def verify_proof(self, proof_id):
        """
        Vérifie une preuve légale.
        
        Args:
            proof_id: ID de la preuve à vérifier
            
        Returns:
            Résultat de la vérification
        """
        # Charger la preuve
        proof_file = os.path.join(self.proofs_dir, f"{proof_id}.json")
        if not os.path.exists(proof_file):
            return {
                "verified": False,
                "error": "Preuve non trouvée"
            }
        
        with open(proof_file, 'r') as f:
            proof = json.load(f)
        
        # Extraire la signature
        signature_b64 = proof.pop("signature", None)
        if not signature_b64:
            return {
                "verified": False,
                "error": "Signature manquante"
            }
        
        # Décoder la signature
        signature = base64.b64decode(signature_b64)
        
        # Recréer la transaction sans la signature pour la vérification
        transaction_str = json.dumps(proof, sort_keys=True).encode()
        
        # Vérifier la signature
        try:
            self.public_key.verify(
                signature,
                transaction_str,
                padding.PSS(
                    mgf=padding.MGF1(hashes.SHA256()),
                    salt_length=padding.PSS.MAX_LENGTH
                ),
                hashes.SHA256()
            )
            signature_valid = True
        except Exception:
            signature_valid = False
        
        # Vérifier que la preuve est dans la chaîne
        proof_in_chain = False
        block_index = None
        
        for i, block in enumerate(self.chain["blocks"]):
            for transaction in block["transactions"]:
                if transaction.get("id") == proof_id:
                    proof_in_chain = True
                    block_index = i
                    break
            if proof_in_chain:
                break
        
        # Restaurer la signature dans la preuve
        proof["signature"] = signature_b64
        
        return {
            "verified": signature_valid and proof_in_chain,
            "signature_valid": signature_valid,
            "in_blockchain": proof_in_chain,
            "block_index": block_index,
            "timestamp": proof.get("timestamp"),
            "data_hash": proof.get("data_hash"),
            "metadata": proof.get("metadata", {})
        }
    
    def get_proof(self, proof_id):
        """
        Récupère une preuve par son ID.
        
        Args:
            proof_id: ID de la preuve
            
        Returns:
            Preuve ou None si non trouvée
        """
        proof_file = os.path.join(self.proofs_dir, f"{proof_id}.json")
        if not os.path.exists(proof_file):
            return None
        
        with open(proof_file, 'r') as f:
            return json.load(f)
    
    def list_proofs(self):
        """
        Liste toutes les preuves disponibles.
        
        Returns:
            Liste des IDs de preuves
        """
        proofs = []
        for filename in os.listdir(self.proofs_dir):
            if filename.endswith(".json"):
                proof_id = filename[:-5]  # Enlever l'extension .json
                proofs.append(proof_id)
        
        return proofs
    
    def verify_chain_integrity(self):
        """
        Vérifie l'intégrité de la chaîne de blocs.
        
        Returns:
            Résultat de la vérification
        """
        blocks = self.chain["blocks"]
        
        for i in range(1, len(blocks)):
            current_block = blocks[i]
            previous_block = blocks[i-1]
            
            # Vérifier le hash précédent
            if current_block["previous_hash"] != self._hash_block(previous_block):
                return {
                    "valid": False,
                    "error": f"Hash incorrect pour le bloc {i}",
                    "block_index": i
                }
            
            # Vérifier la preuve de travail
            if not self._verify_proof_of_work(previous_block["proof"], current_block["proof"]):
                return {
                    "valid": False,
                    "error": f"Preuve de travail invalide pour le bloc {i}",
                    "block_index": i
                }
        
        return {
            "valid": True,
            "blocks_count": len(blocks),
            "last_updated": self.chain["last_updated"]
        }
    
    def _verify_proof_of_work(self, previous_proof, proof):
        """
        Vérifie une preuve de travail.
        
        Args:
            previous_proof: Preuve du bloc précédent
            proof: Preuve à vérifier
            
        Returns:
            True si la preuve est valide, False sinon
        """
        hash_operation = hashlib.sha256(f'{previous_proof**2 - proof**2}'.encode()).hexdigest()
        return hash_operation[:4] == '0000'


class ForensicEvidence:
    """
    Classe pour la gestion des preuves forensiques avec intégration blockchain.
    """
    
    def __init__(self, evidence_dir="./evidence", blockchain=None):
        """
        Initialise le gestionnaire de preuves forensiques.
        
        Args:
            evidence_dir: Répertoire de stockage des preuves
            blockchain: Instance de BlockchainProof (créée si None)
        """
        self.evidence_dir = evidence_dir
        os.makedirs(evidence_dir, exist_ok=True)
        
        if blockchain is None:
            self.blockchain = BlockchainProof()
        else:
            self.blockchain = blockchain
        
        # Registre des preuves
        self.registry_file = os.path.join(evidence_dir, "registry.json")
        self.registry = self._load_registry()
    
    def _load_registry(self):
        """
        Charge le registre des preuves.
        
        Returns:
            Registre des preuves
        """
        if os.path.exists(self.registry_file):
            with open(self.registry_file, 'r') as f:
                return json.load(f)
        else:
            registry = {
                "evidences": [],
                "last_updated": datetime.now().isoformat()
            }
            
            with open(self.registry_file, 'w') as f:
                json.dump(registry, f, indent=2)
            
            return registry
    
    def _save_registry(self):
        """
        Sauvegarde le registre des preuves.
        """
        self.registry["last_updated"] = datetime.now().isoformat()
        
        with open(self.registry_file, 'w') as f:
            json.dump(self.registry, f, indent=2)
    
    def add_evidence(self, evidence_data, metadata=None):
        """
        Ajoute une preuve forensique avec certification blockchain.
        
        Args:
            evidence_data: Données de la preuve
            metadata: Métadonnées supplémentaires
            
        Returns:
            ID de la preuve
        """
        # Générer un ID unique pour la preuve
        evidence_id = hashlib.sha256(f"{str(evidence_data)}:{time.time()}".encode()).hexdigest()
        
        # Préparer les métadonnées
        if metadata is None:
            metadata = {}
        
        metadata.update({
            "timestamp": datetime.now().isoformat(),
            "evidence_id": evidence_id
        })
        
        # Créer la preuve blockchain
        proof_id = self.blockchain.create_proof(evidence_data, metadata)
        
        # Enregistrer la preuve dans le registre
        evidence_entry = {
            "id": evidence_id,
            "proof_id": proof_id,
            "timestamp": metadata["timestamp"],
            "metadata": metadata
        }
        
        self.registry["evidences"].append(evidence_entry)
        self._save_registry()
        
        # Sauvegarder les données de la preuve
        evidence_file = os.path.join(self.evidence_dir, f"{evidence_id}.json")
        with open(evidence_file, 'w') as f:
            json.dump({
                "data": evidence_data,
                "metadata": metadata,
                "proof_id": proof_id
            }, f, indent=2)
        
        return evidence_id
    
    def get_evidence(self, evidence_id):
        """
        Récupère une preuve par son ID.
        
        Args:
            evidence_id: ID de la preuve
            
        Returns:
            Preuve ou None si non trouvée
        """
        evidence_file = os.path.join(self.evidence_dir, f"{evidence_id}.json")
        if not os.path.exists(evidence_file):
            return None
        
        with open(evidence_file, 'r') as f:
            return json.load(f)
    
    def verify_evidence(self, evidence_id):
        """
        Vérifie l'intégrité d'une preuve.
        
        Args:
            evidence_id: ID de la preuve
            
        Returns:
            Résultat de la vérification
        """
        # Récupérer la preuve
        evidence = self.get_evidence(evidence_id)
        if evidence is None:
            return {
                "verified": False,
                "error": "Preuve non trouvée"
            }
        
        # Récupérer l'ID de la preuve blockchain
        proof_id = evidence.get("proof_id")
        if not proof_id:
            return {
                "verified": False,
                "error": "ID de preuve blockchain manquant"
            }
        
        # Vérifier la preuve blockchain
        blockchain_verification = self.blockchain.verify_proof(proof_id)
        
        # Vérifier que le hash des données correspond
        data_str = json.dumps(evidence["data"], sort_keys=True)
        data_hash = hashlib.sha256(data_str.encode()).hexdigest()
        
        hash_match = data_hash == blockchain_verification.get("data_hash", "")
        
        return {
            "verified": blockchain_verification.get("verified", False) and hash_match,
            "blockchain_verification": blockchain_verification,
            "hash_match": hash_match,
            "evidence_id": evidence_id,
            "timestamp": evidence.get("metadata", {}).get("timestamp")
        }
    
    def list_evidences(self):
        """
        Liste toutes les preuves disponibles.
        
        Returns:
            Liste des preuves
        """
        return self.registry["evidences"]
    
    def create_chain_of_custody(self, evidence_id, action, actor, notes=None):
        """
        Ajoute une entrée à la chaîne de custody pour une preuve.
        
        Args:
            evidence_id: ID de la preuve
            action: Action effectuée (collect, analyze, transfer, etc.)
            actor: Personne ayant effectué l'action
            notes: Notes supplémentaires
            
        Returns:
            ID de l'entrée de chaîne de custody
        """
        # Vérifier que la preuve existe
        evidence = self.get_evidence(evidence_id)
        if evidence is None:
            raise ValueError(f"Preuve {evidence_id} non trouvée")
        
        # Créer l'entrée de chaîne de custody
        custody_entry = {
            "evidence_id": evidence_id,
            "action": action,
            "actor": actor,
            "timestamp": datetime.now().isoformat(),
            "notes": notes or ""
        }
        
        # Ajouter l'entrée à la chaîne de custody de la preuve
        if "chain_of_custody" not in evidence:
            evidence["chain_of_custody"] = []
        
        evidence["chain_of_custody"].append(custody_entry)
        
        # Sauvegarder la preuve mise à jour
        evidence_file = os.path.join(self.evidence_dir, f"{evidence_id}.json")
        with open(evidence_file, 'w') as f:
            json.dump(evidence, f, indent=2)
        
        # Créer une preuve blockchain pour cette entrée de chaîne de custody
        custody_id = hashlib.sha256(f"{evidence_id}:{action}:{datetime.now().isoformat()}".encode()).hexdigest()
        custody_entry["id"] = custody_id
        
        self.blockchain.create_proof(custody_entry, {
            "type": "chain_of_custody",
            "evidence_id": evidence_id
        })
        
        return custody_id
    
    def export_evidence_report(self, evidence_id, include_chain_of_custody=True):
        """
        Exporte un rapport complet pour une preuve.
        
        Args:
            evidence_id: ID de la preuve
            include_chain_of_custody: Inclure la chaîne de custody
            
        Returns:
            Rapport au format dictionnaire
        """
        # Récupérer la preuve
        evidence = self.get_evidence(evidence_id)
        if evidence is None:
            raise ValueError(f"Preuve {evidence_id} non trouvée")
        
        # Vérifier l'intégrité
        verification = self.verify_evidence(evidence_id)
        
        # Créer le rapport
        report = {
            "evidence_id": evidence_id,
            "metadata": evidence.get("metadata", {}),
            "verification": verification,
            "blockchain_proof_id": evidence.get("proof_id"),
            "timestamp": evidence.get("metadata", {}).get("timestamp"),
            "export_timestamp": datetime.now().isoformat()
        }
        
        # Ajouter la chaîne de custody si demandé
        if include_chain_of_custody and "chain_of_custody" in evidence:
            report["chain_of_custody"] = evidence["chain_of_custody"]
        
        return report


# Classe de gestion unifiée de la couche blockchain
class BlockchainLayer:
    """
    Couche blockchain unifiée pour IRIS-Forensic X.
    Intègre toutes les fonctionnalités blockchain dans une interface cohérente.
    """
    
    def __init__(self, storage_dir="./blockchain", evidence_dir="./evidence"):
        """
        Initialise la couche blockchain.
        
        Args:
            storage_dir: Répertoire de stockage blockchain
            evidence_dir: Répertoire de stockage des preuves
        """
        self.storage_dir = storage_dir
        self.evidence_dir = evidence_dir
        
        # Créer les répertoires si nécessaire
        os.makedirs(storage_dir, exist_ok=True)
        os.makedirs(evidence_dir, exist_ok=True)
        
        # Initialiser les composants
        self.blockchain = BlockchainProof(storage_dir)
        self.evidence = ForensicEvidence(evidence_dir, self.blockchain)
    
    def create_evidence(self, data, metadata=None):
        """
        Crée une preuve forensique avec certification blockchain.
        
        Args:
            data: Données de la preuve
            metadata: Métadonnées supplémentaires
            
        Returns:
            ID de la preuve
        """
        return self.evidence.add_evidence(data, metadata)
    
    def verify_evidence(self, evidence_id):
        """
        Vérifie l'intégrité d'une preuve.
        
        Args:
            evidence_id: ID de la preuve
            
        Returns:
            Résultat de la vérification
        """
        return self.evidence.verify_evidence(evidence_id)
    
    def add_to_chain_of_custody(self, evidence_id, action, actor, notes=None):
        """
        Ajoute une entrée à la chaîne de custody pour une preuve.
        
        Args:
            evidence_id: ID de la preuve
            action: Action effectuée
            actor: Personne ayant effectué l'action
            notes: Notes supplémentaires
            
        Returns:
            ID de l'entrée de chaîne de custody
        """
        return self.evidence.create_chain_of_custody(evidence_id, action, actor, notes)
    
    def get_evidence(self, evidence_id):
        """
        Récupère une preuve par son ID.
        
        Args:
            evidence_id: ID de la preuve
            
        Returns:
            Preuve ou None si non trouvée
        """
        return self.evidence.get_evidence(evidence_id)
    
    def list_evidences(self):
        """
        Liste toutes les preuves disponibles.
        
        Returns:
            Liste des preuves
        """
        return self.evidence.list_evidences()
    
    def export_evidence_report(self, evidence_id):
        """
        Exporte un rapport complet pour une preuve.
        
        Args:
            evidence_id: ID de la preuve
            
        Returns:
            Rapport au format dictionnaire
        """
        return self.evidence.export_evidence_report(evidence_id)
    
    def verify_blockchain_integrity(self):
        """
        Vérifie l'intégrité de la chaîne de blocs.
        
        Returns:
            Résultat de la vérification
        """
        return self.blockchain.verify_chain_integrity()
    
    def create_direct_proof(self, data, metadata=None):
        """
        Crée une preuve directe dans la blockchain sans passer par le système de preuves forensiques.
        
        Args:
            data: Données à prouver
            metadata: Métadonnées supplémentaires
            
        Returns:
            ID de la preuve
        """
        return self.blockchain.create_proof(data, metadata)
    
    def verify_direct_proof(self, proof_id):
        """
        Vérifie une preuve directe.
        
        Args:
            proof_id: ID de la preuve
            
        Returns:
            Résultat de la vérification
        """
        return self.blockchain.verify_proof(proof_id)
