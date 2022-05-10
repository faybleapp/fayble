import { SanitisePaths } from "helpers/pathHelpers";
import { Library } from "models/api-models";
import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { toast } from "react-toastify";
import { useHttpClient } from "services/httpClient";
import styles from "./LibraryPathsTab.module.scss";

interface LibraryPathTabProps {
	library: Library;
	updateLibrary: (library: Library) => void;
}

export const LibraryPathTab = ({
	library,
	updateLibrary,
}: LibraryPathTabProps) => {
	const [isValidatingPath, setValidatingPath] = useState(false);
	const [newPath, setNewPath] = useState("");
	const client = useHttpClient();

	const removePath = () => {
		updateLibrary({
			...library,
			folderPath: "",
		});
	};

	const pathExists = async (path: string) => {
		return (
			await client.get<boolean>(`/filesystem/pathexists?path=${path}`)
		).data;
	};

	const addPath = async () => {
		setValidatingPath(true);

		const sanitisedPath = SanitisePaths(newPath);

		let valid = false;
		try {
			valid = await pathExists(sanitisedPath);
		} catch (error) {
			toast.error("An error occured while validating path");
			console.log(error);
			setValidatingPath(false);
			return;
		}

		if (!valid) {
			toast.error("Path does not exist or is not accessible");
			setValidatingPath(false);
			return;
		}
		updateLibrary({
			...library,
			folderPath: newPath,
		});

		setNewPath("");
		setValidatingPath(false);
	};

	return (
		<div className={styles.path}>
			{!library.folderPath ? (
				<>
					<Form.Group className={"mb-3"}>
						<InputGroup>
							<Form.Control
								disabled={isValidatingPath}
								onChange={(
									e: React.ChangeEvent<HTMLInputElement>
								): void => setNewPath(e.target.value)}
								value={newPath}
								placeholder="Path"
							/>
							<Button
								disabled={
									isValidatingPath || newPath.trim() === ""
								}
								onClick={() => addPath()}
								variant="outline-secondary">
								{isValidatingPath
									? "Validating..."
									: "Add Path"}
							</Button>
						</InputGroup>
					</Form.Group>
				</>
			) : (
				<InputGroup className={styles.pathItem}>
					<Form.Control
						disabled
						placeholder="Path"
						value={library.folderPath}
					/>
					<Button onClick={() => removePath()} variant="danger">
						X
					</Button>
				</InputGroup>
			)}
		</div>
	);
};
