import {
  faCheckCircle,
  faCircleExclamation
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  ColumnDef,
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable
} from "@tanstack/react-table";
import cn from "classnames";
import {
  IndeterminateCheckbox,
  IndeterminateCheckboxValue
} from "components/indeterminateCheckbox";
import { ComicFile, ImportFile } from "models/api-models";
import { useEffect, useState } from "react";
import { Button, OverlayTrigger, Table, Tooltip } from "react-bootstrap";
import { useAllSeries } from "services";
import { SelectSeriesModal } from "./components/SelectSeriesModal";
import { SetImportFilenameModal } from "./components/SetImportFilenameModal";
import { SetNumberModal } from "./components/SetNumberModal";
import styles from "./ImportTable.module.scss";

interface ImportTableProps {
  files: ComicFile[];
}

export const ImportTable = ({ files }: ImportTableProps) => {
  const [showSeriesModal, setShowSeriesModal] = useState<boolean>(false);
  const [showNumberModal, setShowNumberModal] = useState<boolean>(false);
  const [queuedForImport, setQueuedForImport] = useState<Array<string>>([]);
  const [checkedFiles, setCheckedFiles] = useState<Array<string>>([]);
  const [selectedFile, setSelectedFile] = useState<string | undefined>();
  const [selectAllStatus, setSelectAllStatus] =
    useState<IndeterminateCheckboxValue>(IndeterminateCheckboxValue.Unchecked);
  const [showImportFilenameModal, setShowImportFilenameModal] =
    useState<boolean>(false);
  const [importFiles, setImportFiles] = useState<Array<ImportFile>>(
    files.map((file) => {
      return {
        seriesId: "",
        destinationFileName: file.fileName,
        filePath: file.filePath,
        number: file.number || "",
      };
    })
  );

  const { data: series } = useAllSeries();
  const checkedAndValid = importFiles.filter(
    (f) =>
      checkedFiles.some((cf) => cf === f.filePath) &&
      f.destinationFileName &&
      f.seriesId
  );

  useEffect(() => {
    if (checkedFiles.length === 0) {
      setSelectAllStatus(IndeterminateCheckboxValue.Unchecked);
    } else if (checkedFiles.length === importFiles.length) {
      setSelectAllStatus(IndeterminateCheckboxValue.Checked);
    } else {
      setSelectAllStatus(IndeterminateCheckboxValue.Indeterminate);
    }
  }, [checkedFiles, importFiles]);

  const columnHelper = createColumnHelper<ComicFile>();

  const renderSeriesSelect = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    const seriesName = series?.find((s) => s.id === importFile?.seriesId)?.name;
    return (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !seriesName,
        })}
        onClick={() => {
          setSelectedFile(id);
          setShowSeriesModal(true);
        }}>
        {seriesName ?? "select"}
      </div>
    );
  };

  const renderCheckbox = (id: string) => {
    return !queuedForImport.some((q) => q === id) ? (
      <input
        type="checkbox"
        className={styles.checkbox}
        checked={checkedFiles.includes(id)}
        onChange={(e) => {
          e.target.checked
            ? setCheckedFiles([...checkedFiles, id])
            : setCheckedFiles(checkedFiles.filter((item) => item !== id));
        }}
      />
    ) : null;
  };

  const renderHeaderCheckbox = () => {
    return (
      <IndeterminateCheckbox
        className={styles.checkbox}
        onChange={() => {
          setCheckedFiles(
            selectAllStatus === IndeterminateCheckboxValue.Checked ||
              selectAllStatus === IndeterminateCheckboxValue.Indeterminate
              ? []
              : importFiles.map((i) => i.filePath)
          );
        }}
        value={selectAllStatus}
      />
    );
  };

  const renderImportFilename = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    return (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !importFile?.destinationFileName,
        })}
        onClick={() => {
          setSelectedFile(id);
          setShowImportFilenameModal(true);
        }}>
        {!!importFile?.destinationFileName
          ? importFile?.destinationFileName
          : "set"}
      </div>
    );
  };

  const renderNumber = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    return (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !importFile?.number,
        })}
        onClick={() => {
          setSelectedFile(id);
          setShowNumberModal(true);
        }}>
        {!!importFile?.number ? importFile?.number : "set"}
      </div>
    );
  };

  const renderStatus = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    let error;
    let message = "";

    if (queuedForImport.some((q) => q === importFile?.filePath)) {
      error = false;
      message = "Queued for import";
    } else if (!importFile?.seriesId) {
      error = true;
      message = "You must select a series";
    } else if (!importFile?.destinationFileName) {
      error = true;
      message = "You must set a destination filename";
    } else {
      return null;
    }

    return (
      <OverlayTrigger placement="left" overlay={<Tooltip>{message}</Tooltip>}>
        <div>
          {error ? (
            <FontAwesomeIcon
              className={styles.errorIcon}
              icon={faCircleExclamation}
            />
          ) : (
            <FontAwesomeIcon
              className={styles.importIcon}
              icon={faCheckCircle}
            />
          )}
        </div>
      </OverlayTrigger>
    );
  };

  const handleImport = () => {
    const files = importFiles.filter(
      (f) =>
        checkedFiles.some((cf) => cf === f.filePath) &&
        f.destinationFileName &&
        f.seriesId
    );
    setQueuedForImport(files.map((f) => f.filePath));
    setCheckedFiles([]);
  };

  const columns: ColumnDef<ComicFile, any>[] = [
    columnHelper.accessor((row) => row, {
      id: "check",
      header: () => renderHeaderCheckbox(),
      cell: (info) => renderCheckbox(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row.fileName, {
      id: "filename",
      header: "Filename",
      cell: (info) => info.getValue(),
    }),
    columnHelper.accessor((row) => row, {
      header: "Series",
      cell: (info) => renderSeriesSelect(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      header: "Number",
      cell: (info) => renderNumber(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      header: "Import Filename",
      cell: (info) => renderImportFilename(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      id: "status",
      header: "",
      cell: (info) => renderStatus(info.row.original.filePath),
    }),
  ];

  const table = useReactTable({
    data: files,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

useEffect(() => {
  console.log(selectedFile);
}, [selectedFile])

  return (
    <>
      <div className={styles.importTable}>
        <Table striped hover>
          <thead>
            {table.getHeaderGroups()?.map((headerGroup) => (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <th key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody className={styles.tableBody}>
            {table.getRowModel().rows.map((row) => (
              <tr key={row.id}>
                {row.getVisibleCells().map((cell) => (
                  <td className={styles.tableCell} key={cell.id}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </Table>
      </div>
      <div className={styles.tableButtons}>
        <Button
          disabled={checkedFiles.length === 0}
          variant="secondary"
          onClick={() => setShowSeriesModal(true)}>
          Select Series
        </Button>
        <Button
          disabled={checkedAndValid.length === 0}
          className={styles.importButton}
          onClick={handleImport}>
          Import Selected
        </Button>
      </div>
      <SelectSeriesModal
        show={showSeriesModal}
        onClose={() => {
          setShowSeriesModal(false);
          setSelectedFile(undefined);
        }}
        onSelectSeries={(seriesId) => {
          setImportFiles(
            importFiles.map((file) => {
              // Process single file
              if (selectedFile) {
                if (file.filePath === selectedFile) {
                  return { ...file, seriesId: seriesId };
                }
                return file;
              }
              // Process checked files
              if (checkedFiles.some((f) => f === file.filePath)) {
                return { ...file, seriesId: seriesId };
              }
              return file;
            })
          );
          setShowSeriesModal(false);
          setSelectedFile(undefined);
        }}
      />
      <SetNumberModal
        number={
          importFiles.find((f) => f.filePath === selectedFile)?.number || ""
        }
        show={showNumberModal}
        onClose={() => {
          setShowNumberModal(false);
          setSelectedFile(undefined);
        }}
        onChange={(number) => {
          setImportFiles(
            importFiles.map((file) => {
              if (file.filePath === selectedFile) {
                return { ...file, number: number };
              }
              return file;
            })
          );
          setShowSeriesModal(false);
        }}
      />

      <SetImportFilenameModal
        onClose={() => {
          setShowImportFilenameModal(false);
          setSelectedFile(undefined);
        }}
        show={showImportFilenameModal}
        filename={
          importFiles.find((f) => f.filePath === selectedFile)
            ?.destinationFileName || ""
        }
        onChange={(fileName) => {
          setImportFiles(
            importFiles.map((file) => {
              if (file.filePath === selectedFile) {
                return { ...file, destinationFileName: fileName };
              }
              return file;
            })
          );
        }}
      />
    </>
  );
};