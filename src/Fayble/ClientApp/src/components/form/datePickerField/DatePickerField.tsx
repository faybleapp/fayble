import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import { LockableField } from "models/ui-models";
import { Form, InputGroup } from "react-bootstrap";
import { useFormContext, useWatch } from "react-hook-form";

interface DatePickerFieldProps extends LockableField {
  name: string;
  label: string;
  className?: string;
  type?: "date" | "month";
}

export const DatePickerField = ({
  name,
  label,
  className,
  lockable,
  type,
}: DatePickerFieldProps) => {
  const {
    register,
    setValue,
    control,
    formState: { errors },
  } = useFormContext();

  const locked = useWatch({ control, name: `fieldLocks.${name}` });
  const field = register(name);
  return (
    <Form.Group className={cn(className, "mb-3")}>
      <Form.Label>{label}</Form.Label>
      <InputGroup>
        <Form.Control
          {...field}
          max="9999-12-31"
          type={type ?? "date"}
          isInvalid={!!errors.name}
          onChange={(e: any) => {
            if (lockable) {
              setValue(`fieldLocks.${name}`, true, { shouldDirty: true });
            }
            field.onChange(e);
          }}
        />
        {lockable && (
          <FieldLock
            locked={locked}
            onClick={() =>
              setValue(`fieldLocks.${name}`, !locked, { shouldDirty: true })
            }
          />
        )}
        <Form.Control.Feedback type="invalid">
          {errors.name?.message?.toString()}
        </Form.Control.Feedback>
      </InputGroup>
    </Form.Group>
  );
};
