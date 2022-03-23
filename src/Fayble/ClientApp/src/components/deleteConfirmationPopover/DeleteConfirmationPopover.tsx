import React from "react";
import { Button, OverlayTrigger, Popover } from "react-bootstrap";
import styles from "./DeleteConfirmationPopover.module.scss";

interface DeleteConfirmationPopoverProps {
	placement: "top" | "right" | "bottom" | "left";
	title: string;
	onConfirmation: () => void;
	children: React.ReactElement;
}

export const DeleteConfirmationPopover =
	({placement, title, onConfirmation, children}: DeleteConfirmationPopoverProps) => {
		const handleDelete = () => {
			document.body.click();
			onConfirmation();
		};
		const popoverDelete = (
			<Popover id="delete-confirmation-popover">
				<Popover.Header>{title}</Popover.Header>
				<Popover.Body>
					Are you sure you want to delete this library?
					<div className={styles.buttonContainer}>
						<Button
							variant="secondary"
							className={styles.button}
							onClick={() => document.body.click()}>
							Cancel
						</Button>
						<div className={styles.separator} />
						<Button
							variant="danger"
							onClick={() => handleDelete()}
							className={styles.button}>
							Delete
						</Button>
					</div>
				</Popover.Body>
			</Popover>
		);

		return (
			<OverlayTrigger
				rootClose={true}
				trigger="click"
				placement={placement}
				overlay={popoverDelete}>
				{children}
			</OverlayTrigger>
		);
	};
