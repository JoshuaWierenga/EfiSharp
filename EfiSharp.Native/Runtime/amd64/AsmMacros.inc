;;
;; Licensed to the .NET Foundation under one or more agreements.
;; The .NET Foundation licenses this file to you under the MIT license.
;;
;; Changes made by Joshua Wierenga.

;;
;; MACROS
;;

;
; Define macros to build unwind data for prologues.
;

alloc_stack macro Size

        .errnz ___STACK_ADJUSTMENT_FORBIDDEN, alloc_stack cannot be used after save_reg_postrsp

        sub     rsp, Size
        .allocstack Size

        endm

save_reg_postrsp macro Reg, Offset

        .errnz ___FRAME_REG_SET, save_reg_postrsp cannot be used after set_frame

        mov     Offset[rsp], Reg
        .savereg Reg, Offset

        ___STACK_ADJUSTMENT_FORBIDDEN = 1

        endm

save_xmm128_postrsp macro Reg, Offset

        .errnz ___FRAME_REG_SET, save_reg_postrsp cannot be used after set_frame

        movdqa  Offset[rsp], Reg
        .savexmm128 Reg, Offset

        ___STACK_ADJUSTMENT_FORBIDDEN = 1

        endm

END_PROLOGUE macro

        .endprolog

        endm

;
; Define function entry/end macros.
;

LEAF_ENTRY macro Name, Section

Section segment para 'CODE'

        align   16

        public  Name
Name    proc

        endm

LEAF_END macro Name, section

Name    endp

Section ends

        endm


NESTED_ENTRY macro Name, Section, Handler

Section segment para 'CODE'

        align   16

        public  Name

ifb <Handler>

Name    proc    frame

else

Name    proc    frame:Handler

endif

        ___FRAME_REG_SET = 0
        ___STACK_ADJUSTMENT_FORBIDDEN = 0

        endm

NESTED_END macro Name, section

Name    endp

Section ends

        endm


ALTERNATE_ENTRY macro Name

Name label proc
PUBLIC Name
        endm

EXPORT_POINTER_TO_ADDRESS macro Name

        local   AddressToExport

AddressToExport  label   proc

        .const

        align   8

Name    dq      offset AddressToExport

        public  Name

        .code

        endm

;  - TAILCALL_RAX: ("jmp rax") should be used for tailcalls, this emits an instruction
;            sequence which is recognized by the unwinder as a valid epilogue terminator
TAILJMP_RAX TEXTEQU <DB 048h, 0FFh, 0E0h>