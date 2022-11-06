import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import { Tooltip } from "components/tooltip";
import {
  LockableField,
  OptionTypeBase,
  SelectFieldOption
} from "models/ui-models";
import { Form, InputGroup } from "react-bootstrap";
import { Controller, useFormContext } from "react-hook-form";
import Select, { StylesConfig } from "react-select";

import styles from "./SelectField.module.scss";

interface SelectFieldProps extends LockableField {
  name: string;
  label: string;
  className?: string;
  disabled?: boolean;
  clearable?: boolean;
  searchable?: boolean;
  value?: string;
  placeholder?: string;
  tooltip?: string;
  options: SelectFieldOption[] | [];
}

export const SelectField = ({
  label,
  name,
  disabled,
  lockable,
  className,
  clearable,
  searchable,
  tooltip,
  options,
}: SelectFieldProps) => {
  const {
    register,
    setValue,
    control,
    watch,
    formState: { errors },
  } = useFormContext();

  const locked = watch(`fieldLocks.${name}`);
  const field = register(name);

  const selectStyle: StylesConfig<OptionTypeBase, boolean> = {
    control: (provided) => ({
      ...provided,
      backgroundColor: styles.background,
      border: styles.border,
      //boxShadow: styles.none,
    }),
    input: (provided) => ({
      ...provided,
      color: styles.color,
    }),
    option: (provided) => ({
      ...provided,
      backgroundColor: styles.secondaryBackground,
      color: styles.color,
      ":hover": {
        color: styles.accentColor,
      },
      ":active": {
        background: styles.none,
      },
    }),
    singleValue: (provided) => ({
      ...provided,
      color: styles.color,
    }),
    valueContainer: (provided) => ({
      ...provided,
      backgroundColor: styles.background,
    }),
    menuList: (provided) => ({
      ...provided,
      backgroundColor: styles.secondaryBackground,
    }),
  };

  return (
    <Form.Group className="mb-3">
      <div className={styles.labelContainer}>
        {label && <Form.Label>{label}</Form.Label>}
        {tooltip && <Tooltip tooltip={tooltip} />}
      </div>
      <InputGroup>
        <Controller
          name={name}
          control={control}
          render={({ field: { onChange, value, name, ref } }) => (
            <Select
              {...field}
              isDisabled={disabled}
              className={cn(className, styles.select, {
                [styles.lockable]: lockable,
              })}
              styles={selectStyle}
              isClearable={clearable === true}
              isSearchable={searchable === true}
              value={options.find((option) => option.value === value)}
              options={options}
              onChange={(option) => {
                if (lockable) {
                  setValue(`fieldLocks.${name}`, true, { shouldDirty: true });
                }
                onChange(option.value);
              }}
            />
          )}
        />
        {lockable && (
          <FieldLock
            locked={locked}
            onClick={() =>
              setValue(`fieldLocks.${name}`, !locked, { shouldDirty: true })
            }
          />
        )}
      </InputGroup>
    </Form.Group>
  );
};
