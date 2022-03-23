import { SanitisePaths } from "helpers/pathHelpers";
import { Library } from "models/api-models";
import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { toast } from "react-toastify";
import { useHttpClient } from "services/httpClient";
import styles from "./LibraryPathsTab.module.scss";

interface LibraryPathsTabProps {
	library: Library;
	updateLibrary: (library: Library) => void;
}

export const LibraryPathsTab = ({
	library,
	updateLibrary,
}: LibraryPathsTabProps) => {
	const [isValidatingPath, setValidatingPath] = useState(false);
	const [newPath, setNewPath] = useState("");
	const client = useHttpClient();

	const removePath = (path: string) => {
		updateLibrary({
			...library,
			paths: library && library.paths?.filter((item) => item !== path),
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

		if (
			(
				library?.paths?.filter(
					(path) =>
						path?.toLowerCase() === sanitisedPath.toLowerCase()
				) ?? []
			)?.length > 0
		) {
			toast.warn("Path already in list");
			setValidatingPath(false);
			return;
		}

		updateLibrary({
			...library,
			paths: [sanitisedPath, ...(library.paths || [])],
		});

		setNewPath("");
		setValidatingPath(false);
	};

	return (
		<>
			<Form.Group className={"mb-3"}>
				<Form.Label>Library Path</Form.Label>
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
						disabled={isValidatingPath || newPath.trim() === ""}
						onClick={() => addPath()}
						variant="outline-secondary">
						{isValidatingPath ? "Validating..." : "Add Path"}
					</Button>
				</InputGroup>
			</Form.Group>
			<div className={styles.pathList}>
				{library &&
					library.paths?.map((path: string, index: number) => {
						return (
							<InputGroup key={index} className={styles.pathItem}>
								<Form.Control
									disabled
									placeholder="Path"
									value={path}
								/>
								<Button
									onClick={() => removePath(path!)}
									variant="danger">
									X
								</Button>
							</InputGroup>
						);
					})}
			</div>
		</>
	);
};
