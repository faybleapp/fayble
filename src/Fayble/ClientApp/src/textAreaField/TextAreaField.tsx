import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import { LockableField } from "models/ui-models";
import React from "react";
import { Form, InputGroup } from "react-bootstrap";

interface TextAreaFieldProps extends LockableField {
	name: string;
	label: string;
	className?: string;
	rows: number;
	error?: string | false | undefined;
	value?: string;
	onChange: (event: React.ChangeEvent<unknown>) => void;
}

export const TextAreaField: React.FC<TextAreaFieldProps> = ({
	name,
	label,
	className,
	rows,
	error,
	value,
	onChange,
	onLock,
	locked = false,
}) => {
	return (
		<Form.Group className={cn(className, "mb-3")}>
			<Form.Label>{label}</Form.Label>
			<InputGroup>
				<Form.Control
					name={name}
					rows={rows}
					as="textarea"
					isInvalid={!!error}
					value={value || ""}
					onChange={(e: any) => {
						if (onLock) {
							onLock(true);
						}
						onChange(e);
					}}
				/>
				{onLock && <FieldLock locked={locked} onClick={onLock} />}
			</InputGroup>
			<Form.Control.Feedback type="invalid">
				{error}
			</Form.Control.Feedback>
		</Form.Group>
	);
};
