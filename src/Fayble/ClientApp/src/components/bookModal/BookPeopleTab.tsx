import { MultiSelectField } from "components/multiSelectField";
import { BookFieldLocks, BookPerson, Person } from "models/api-models";
import { RoleType } from "models/ui-models";
import { Container } from "react-bootstrap";
import { useFormContext, useWatch } from "react-hook-form";
import { usePeople } from "services/person";

export const BookPeopleTab = () => {
  const { setValue, control } = useFormContext();
  const bookPeople: BookPerson[] = useWatch({ control, name: "people" });
  const fieldLocks: BookFieldLocks = useWatch({ control, name: "fieldLocks" });

  var people = usePeople();
  var peopleOptions =
    Array.from(
      new Set(
        people?.data
          ?.map((person: Person) => person.name)
          .concat(bookPeople?.map((person: BookPerson) => person.name) || []) ||
          []
      )
    ).map((person) => ({
      value: person,
      label: person,
    })) || [];

  const handleOnChange = (role: RoleType, selectedValues: string[]) => {
    const newPeople =
      selectedValues?.map((value) => ({
        name: value,
        role: role,
      })) || [];
    setValue(
      "people",
      (bookPeople || [])
        .filter((people: BookPerson) => people.role !== role)
        .concat(newPeople as BookPerson[])
    );
  };

  const getPeopleValues = (role: RoleType) => {
    return (
      bookPeople
        ?.filter((person) => person.role === role)
        .map((person) => person.name) || []
    );
  };

  return (
    <Container>
      <MultiSelectField
        name="writers"
        creatable
        clearable
        locked={fieldLocks.writers}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.writers", lock, { shouldDirty: true })
        }
        value={getPeopleValues(RoleType.Writer)}
        label="Writers"
        onChange={(selectedValues) => {
          handleOnChange(RoleType.Writer, selectedValues);
        }}
        options={peopleOptions}
      />
      <MultiSelectField
        name="inkers"
        creatable
        clearable
        locked={fieldLocks.inkers}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.inkers", lock, { shouldDirty: true })
        }
        value={getPeopleValues(RoleType.Inker)}
        label="Inkers"
        onChange={(selectedValues) => {
          handleOnChange(RoleType.Inker, selectedValues);
        }}
        options={peopleOptions}
      />
      <MultiSelectField
        name="editors"
        creatable
        clearable
        locked={fieldLocks.editors}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.editors", lock, { shouldDirty: true })
        }
        value={getPeopleValues(RoleType.Editor)}
        label="Editors"
        onChange={(selectedValues) => {
          handleOnChange(RoleType.Editor, selectedValues);
        }}
        options={peopleOptions}
      />
      <MultiSelectField
        name="pencillers"
        creatable
        clearable
        locked={fieldLocks.pencillers}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.pencillers", lock, { shouldDirty: true })
        }
        value={getPeopleValues(RoleType.Penciller)}
        label="Pencillers"
        onChange={(selectedValues) => {
          handleOnChange(RoleType.Penciller, selectedValues);
        }}
        options={peopleOptions}
      />
      <MultiSelectField
        name="letterers"
        creatable
        clearable
        locked={fieldLocks.letterers}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.letterers", lock, { shouldDirty: true })
        }
        value={getPeopleValues(RoleType.Letterer)}
        label="Letterers"
        onChange={(selectedValues) => {
          handleOnChange(RoleType.Letterer, selectedValues);
        }}
        options={peopleOptions}
      />
      <MultiSelectField
        name="colorists"
        creatable
        clearable
        locked={fieldLocks.colorists}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.colorists", lock, { shouldDirty: true })
        }
        value={getPeopleValues(RoleType.Colorist)}
        label="Colorists"
        onChange={(selectedValues) => {
          handleOnChange(RoleType.Colorist, selectedValues);
        }}
        options={peopleOptions}
      />
      <MultiSelectField
        name="coverArtists"
        creatable
        clearable
        locked={fieldLocks.coverArtists}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.coverArtists", lock, { shouldDirty: true })
        }
        value={getPeopleValues(RoleType.CoverArtist)}
        label="Cover Artists"
        onChange={(selectedValues) => {
          handleOnChange(RoleType.CoverArtist, selectedValues);
        }}
        options={peopleOptions}
      />
      <MultiSelectField
        name="other"
        creatable
        clearable
        locked={fieldLocks.other}
        onLock={(lock: boolean) =>
          setValue("fieldLocks.other", lock, { shouldDirty: true })
        }
        value={getPeopleValues(RoleType.Other)}
        label="Other"
        onChange={(selectedValues) => {
          handleOnChange(RoleType.Other, selectedValues);
        }}
        options={peopleOptions}
      />
    </Container>
  );
};
