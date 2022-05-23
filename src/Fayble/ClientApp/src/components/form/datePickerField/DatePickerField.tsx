import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import { LockableField } from "models/ui-models";
import React from "react";
import { Form, InputGroup } from "react-bootstrap";

interface DatePickerFieldProps extends LockableField {
	name: string;
	label: string;
	className?: string;
	error?: string | false | undefined;
	value?: string;
	type?: "date" | "month";
	onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export const DatePickerField = ({
	name,
	label,
	className,
	error,
	value,
	type,
	onChange,
	onLock,
	locked = false,
}: DatePickerFieldProps) => {
	return (
		<Form.Group className={cn(className, "mb-3")}>
			<Form.Label>{label}</Form.Label>
			<InputGroup>
				<Form.Control
					name={name}
					max="9999-12-31"
					type={type ?? "date"}
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
