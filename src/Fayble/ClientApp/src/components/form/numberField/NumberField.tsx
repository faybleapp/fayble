import { FieldLock } from "components/fieldLock";
import { LockableField } from "models/ui-models";
import { Form, InputGroup } from "react-bootstrap";
import { useFormContext, useWatch } from "react-hook-form";

interface NumberFieldProps extends LockableField {
  name: string;
  label?: string;
  className?: string;
  placeholder?: string;
}

export const NumberField = ({
  name,
  label,
  className,
  placeholder,
  lockable,
}: NumberFieldProps) => {
  const {
    register,
    setValue,
    control,
    formState: { errors },
  } = useFormContext();

  const locked = useWatch({ control, name: `fieldLocks.${name}` });
  const field = register(name);

  return (
    <Form.Group className={className}>
      {label && <Form.Label>{label}</Form.Label>}
      <InputGroup>
        <Form.Control
          {...field}
          type="number"
          isInvalid={!!errors[name]}
          placeholder={placeholder}
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
          {errors[name]?.message?.toString()}
        </Form.Control.Feedback>
      </InputGroup>
    </Form.Group>
  );
};
